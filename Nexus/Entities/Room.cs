using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Interactables.Interobjects.DoorUtils;

using NorthwoodLib.Pools;

using Nexus.Enums;
using Nexus.Extensions;

namespace Nexus.Entities
{
    /// <summary>
    /// The in-game room.
    /// </summary>
    public class Room : MonoBehaviour
    {
        /// <summary>
        /// Gets the <see cref="Room"/> name.
        /// </summary>
        public string Name => name;

        /// <summary>
        /// Gets the <see cref="Room"/> <see cref="UnityEngine.Transform"/>.
        /// </summary>
        public Transform Transform => transform;

        /// <summary>
        /// Gets the <see cref="Room"/> position.
        /// </summary>
        public Vector3 Position => transform.position;

        /// <summary>
        /// Gets the <see cref="ZoneType"/> in which the room is located.
        /// </summary>
        public ZoneType Zone { get; private set; }

        /// <summary>
        /// Gets the <see cref="RoomType"/>.
        /// </summary>
        public RoomType Type { get; private set; }

        /// <summary>
        /// Gets a <see cref="IReadOnlyCollection{T}"/> of <see cref="Player"/> in the <see cref="Room"/>.
        /// </summary>
        public IEnumerable<Player> Players => PlayersList.Get(player => player.Room.Transform == Transform);

        /// <summary>
        /// Gets a <see cref="IEnumerable{T}"/> of <see cref="Door"/> in the <see cref="Room"/>.
        /// </summary>
        public IReadOnlyList<Door> Doors { get; private set; }

        /// <summary>
        /// Gets a value indicating whether or not the lights in this room are currently flickered off.
        /// </summary>
        public bool LightsOff => FlickerableLightController && FlickerableLightController.IsEnabled();

        private FlickerableLightController FlickerableLightController { get; set; }

        /// <summary>
        /// Flickers the room's lights off for a duration.
        /// </summary>
        /// <param name="duration">Duration in seconds.</param>
        public void TurnOffLights(float duration) 
            => FlickerableLightController.ServerFlickerLights(duration);

        /// <summary>
        /// Sets the intensity of the lights in the room.
        /// </summary>
        /// <param name="intensity">The light intensity multiplier. Cannot be brighter than 2 or darker than 0.</param>
        public void SetLightIntensity(float intensity) 
            => FlickerableLightController.Network_lightIntensityMultiplier = intensity;

        private static RoomType FindType(string rawName)
        {
            rawName = rawName.RemoveBracketsOnEndOfName();

            switch (rawName)
            {
                case "LCZ_Armory":
                    return RoomType.LczArmory;
                case "LCZ_Curve":
                    return RoomType.LczCurve;
                case "LCZ_Straight":
                    return RoomType.LczStraight;
                case "LCZ_012":
                    return RoomType.Lcz012;
                case "LCZ_914":
                    return RoomType.Lcz914;
                case "LCZ_Crossing":
                    return RoomType.LczCrossing;
                case "LCZ_TCross":
                    return RoomType.LczTCross;
                case "LCZ_Cafe":
                    return RoomType.LczCafe;
                case "LCZ_Plants":
                    return RoomType.LczPlants;
                case "LCZ_Toilets":
                    return RoomType.LczToilets;
                case "LCZ_Airlock":
                    return RoomType.LczAirlock;
                case "LCZ_173":
                    return RoomType.Lcz173;
                case "LCZ_ClassDSpawn":
                    return RoomType.LczClassDSpawn;
                case "LCZ_ChkpB":
                    return RoomType.LczChkpB;
                case "LCZ_372":
                    return RoomType.LczGlassBox;
                case "LCZ_ChkpA":
                    return RoomType.LczChkpA;
                case "HCZ_079":
                    return RoomType.Hcz079;
                case "HCZ_EZ_Checkpoint":
                    return RoomType.HczEzCheckpoint;
                case "HCZ_Room3ar":
                    return RoomType.HczArmory;
                case "HCZ_Testroom":
                    return RoomType.Hcz939;
                case "HCZ_Hid":
                    return RoomType.HczHid;
                case "HCZ_049":
                    return RoomType.Hcz049;
                case "HCZ_ChkpA":
                    return RoomType.HczChkpA;
                case "HCZ_Crossing":
                    return RoomType.HczCrossing;
                case "HCZ_106":
                    return RoomType.Hcz106;
                case "HCZ_Nuke":
                    return RoomType.HczNuke;
                case "HCZ_Tesla":
                    return RoomType.HczTesla;
                case "HCZ_Servers":
                    return RoomType.HczServers;
                case "HCZ_ChkpB":
                    return RoomType.HczChkpB;
                case "HCZ_Room3":
                    return RoomType.HczTCross;
                case "HCZ_457":
                    return RoomType.Hcz096;
                case "HCZ_Curve":
                    return RoomType.HczCurve;
                case "EZ_Endoof":
                    return RoomType.EzVent;
                case "EZ_Intercom":
                    return RoomType.EzIntercom;
                case "EZ_GateA":
                    return RoomType.EzGateA;
                case "EZ_PCs_small":
                    return RoomType.EzDownstairsPcs;
                case "EZ_Curve":
                    return RoomType.EzCurve;
                case "EZ_PCs":
                    return RoomType.EzPcs;
                case "EZ_Crossing":
                    return RoomType.EzCrossing;
                case "EZ_CollapsedTunnel":
                    return RoomType.EzCollapsedTunnel;
                case "EZ_Smallrooms2":
                    return RoomType.EzConference;
                case "EZ_Straight":
                    return RoomType.EzStraight;
                case "EZ_Cafeteria":
                    return RoomType.EzCafeteria;
                case "EZ_upstairs":
                    return RoomType.EzUpstairsPcs;
                case "EZ_GateB":
                    return RoomType.EzGateB;
                case "EZ_Shelter":
                    return RoomType.EzShelter;
                case "PocketWorld":
                    return RoomType.Pocket;
                case "Outside":
                    return RoomType.Surface;
                default:
                    return RoomType.Unknown;
            }
        }

        private static ZoneType FindZone(GameObject gameObject)
        {
            var transform = gameObject.transform;

            if (transform.parent == null)
                return ZoneType.Unspecified;

            switch (transform.parent.name)
            {
                case "HeavyRooms":
                    return ZoneType.HeavyContainment;
                case "LightRooms":
                    return ZoneType.LightContainment;
                case "EntranceRooms":
                    return ZoneType.Entrance;
                default:
                    return transform.position.y > 900 ? ZoneType.Surface : ZoneType.Unspecified;
            }
        }

        private static IReadOnlyList<Door> FindDoors(GameObject gameObject)
        {
            List<DoorVariant> doorList = ListPool<DoorVariant>.Shared.Rent();

            foreach (Scp079Interactable scp079Interactable in Interface079.singleton.allInteractables)
            {
                if (scp079Interactable.optionalObject != null && scp079Interactable.optionalObject.name == gameObject.name 
                    && scp079Interactable.optionalParameter == gameObject.transform.parent.name)
                {
                    if (scp079Interactable.type == Scp079Interactable.InteractableType.Door)
                    {
                        DoorVariant door = scp079Interactable.GetComponent<DoorVariant>();

                        if (!doorList.Contains(door))
                        {
                            doorList.Add(door);
                        }
                    }
                }
            }

            List<Door> doors = new List<Door>();

            foreach (DoorVariant door in doorList)
            {
                Door door1 = Door.Get(door);

                if (door1 != null)
                    doors.Add(door1);
            }

            ListPool<DoorVariant>.Shared.Return(doorList);

            return doors.AsReadOnly();
        }

        private void Start()
        {
            Zone = FindZone(gameObject);
            Type = FindType(gameObject.name);
            Doors = FindDoors(gameObject);
            FlickerableLightController = GetComponentInChildren<FlickerableLightController>();
        }
    }
}