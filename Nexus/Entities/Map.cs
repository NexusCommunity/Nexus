using System.Collections.Generic;
using System.Linq;
using System;

using Nexus.Entities.Pickups.Base;
using Nexus.Entities.Items.Base;
using Nexus.Attributes;
using Nexus.Events;
using Nexus.Enums;

using Interactables.Interobjects.DoorUtils;
using InventorySystem.Items.Firearms.Utilities;
using InventorySystem.Items.Firearms.Attachments;
using MapGeneration.Distributors;
using NorthwoodLib.Pools;
using UnityEngine;
using Interactables.Interobjects;
using Interactables;
using MapGeneration;

namespace Nexus.Entities
{
    /// <summary>
    /// A class for map management.
    /// </summary>
    public class Map
    {
        #region Internal Fields Stuff

        internal static List<Door> doors;
        internal static List<Camera> cams;
        internal static List<Room> rooms;
        internal static List<Locker> lockers;
        internal static List<Chamber> chambers;
        internal static List<Tesla> tesla;
        internal static List<Generator> generators;
        internal static List<GameObject> tantrums;
        internal static List<Workstation> stations;
        internal static List<Window> windows;
        internal static List<Ragdoll> ragdolls;
        internal static List<Target> targets;
        internal static List<Elevator> elevators;
        internal static List<ElevatorChamber> elevatorObjects;

        internal static PocketDimension pocket;
        internal static TeslaGateController controller;
        internal static Intercom icom;
        internal static Scp914 scp914;
        internal static Recontainer079 recontain;
        internal static LureSubjectContainer lsc;
        internal static GameObject femurBreaker;

        internal static RaycastHit[] cache;

        #endregion

        #region Static Constructor

        static Map()
        {
            doors = new List<Door>();
            cams = new List<Camera>();
            rooms = new List<Room>();
            lockers = new List<Locker>();
            chambers = new List<Chamber>();
            tesla = new List<Tesla>();
            generators = new List<Generator>();
            stations = new List<Workstation>();
            windows = new List<Window>();
            ragdolls = new List<Ragdoll>();
            targets = new List<Target>();
            elevators = new List<Elevator>();
            elevatorObjects = new List<ElevatorChamber>();
            tantrums = new List<GameObject>();

            cache = new RaycastHit[1];
        }

        #endregion

        /// <summary>
        /// Gets the <see cref="GameObject.tag"/> for the surface zone.
        /// </summary>
        public const string SurfaceTag = "Outside";

        /// <summary>
        /// Gets the <see cref="GameObject.tag"/> for the player object.
        /// </summary>
        public const string PlayerTag = "Player";

        /// <summary>
        /// Gets the <see cref="GameObject.tag"/> for the pocket dimension.
        /// </summary>
        public const string PocketDimensionTag = "HeavyRooms/PocketWorld";

        /// <summary>
        /// Gets the <see cref="GameObject.tag"/> for the room object.
        /// </summary>
        public const string RoomTag = "Room";

        /// <summary>
        /// Gets the doors on this map.
        /// </summary>
        public static IReadOnlyList<Door> Doors => doors;

        /// <summary>
        /// Gets the cameras on this map.
        /// </summary>
        public static IReadOnlyList<Camera> Cameras => cams;

        /// <summary>
        /// Gets the rooms on this map.
        /// </summary>
        public static IReadOnlyList<Room> Rooms => rooms;

        /// <summary>
        /// Gets the lockers on this map.
        /// </summary>
        public static IReadOnlyList<Locker> Lockers => lockers;

        /// <summary>
        /// Gets the chambers on this map.
        /// </summary>
        public static IReadOnlyList<Chamber> Chambers => chambers;

        /// <summary>
        /// Gets the gates on this map.
        /// </summary>
        public static IReadOnlyList<Tesla> Gates => tesla;

        /// <summary>
        /// Gets the generators on this map.
        /// </summary>
        public static IReadOnlyList<Generator> Generators => generators;

        /// <summary>
        /// Gets all tantrums on the map.
        /// </summary>
        public static IReadOnlyList<GameObject> Tantrums => tantrums;

        /// <summary>
        /// Gets the workstations on this map.
        /// </summary>
        public static IReadOnlyList<Workstation> Workstations => stations;

        /// <summary>
        /// Gets the windows on this map.
        /// </summary>
        public static IReadOnlyList<Window> Windows => windows;

        /// <summary>
        /// Gets the items on this map.
        /// </summary>
        public static IReadOnlyList<BaseItem> Items => BaseItem.items;

        /// <summary>
        /// Gets the pickups on this map.
        /// </summary>
        public static IReadOnlyList<BasePickup> Pickups => BasePickup.pickups;

        /// <summary>
        /// Gets the ragdolls on this map.
        /// </summary>
        public static IReadOnlyList<Ragdoll> Ragdolls => ragdolls;

        /// <summary>
        /// Gets the pocket dimension exits.
        /// </summary>
        public static IReadOnlyList<PocketDimensionExit> PocketExits => PocketDimension?.Exits ?? null;

        /// <summary>
        /// Gets a list of all shooting targets on the map.
        /// </summary>
        public static IReadOnlyList<Target> Targets
        {
            get
            {
                UpdateTargets();

                return targets;
            }
        }

        /// <summary>
        /// Gets all <see cref="FlickerableLightController"/> instances on the server.
        /// </summary>
        public static IReadOnlyList<FlickerableLightController> LightControllers => FlickerableLightController.Instances;

        /// <summary>
        /// Gets all elevators on the map.
        /// </summary>
        public static IReadOnlyList<Elevator> Elevators => elevators;

        /// <summary>
        /// Gets the <see cref="global::TeslaGateController"/>.
        /// </summary>
        public static TeslaGateController TeslaGateController => controller;

        /// <summary>
        /// Gets the pocket dimension.
        /// </summary>
        public static PocketDimension PocketDimension => pocket;

        /// <summary>
        /// Gets the <see cref="Entities.Intercom"/> instance.
        /// </summary>
        public static Intercom Intercom => icom;

        /// <summary>
        /// Gets the <see cref="Entities.Scp914"/> instance.
        /// </summary>
        public static Scp914 Scp914 => scp914;

        /// <summary>
        /// Gets the <see cref="Recontainer079"/> instance.
        /// </summary>
        public static Recontainer079 Recontainer => recontain;

        /// <summary>
        /// Gets the <see cref="global::LureSubjectContainer"/> instance.
        /// </summary>
        public static LureSubjectContainer LureSubjectContainer => lsc;

        /// <summary>
        /// Gets the Femur Breaker.
        /// </summary>
        public static GameObject FemurBreaker => femurBreaker;

        /// <summary>
        /// Gets or sets a value indicating whether the SCP-106 container was used or not.
        /// </summary>
        public static bool Scp106Contained { get => OneOhSixContainer.used; set => OneOhSixContainer.used = value; }

        /// <summary>
        /// Gets all pickups of the specified type.
        /// </summary>
        /// <typeparam name="T">The pickup type to get.</typeparam>
        /// <returns>Found pickups.</returns>
        public static List<T> GetPickups<T>() where T : BasePickup
            => Pickups.Where(x => x is T).Cast<T>().ToList();

        /// <summary>
        /// Attempts to find an array of components.
        /// </summary>
        /// <typeparam name="T">The component to find.</typeparam>
        /// <returns>The array of components found, if any.</returns>
        public static T[] FindArray<T>() where T : Component
            => UnityEngine.Object.FindObjectsOfType<T>();

        /// <summary>
        /// Attempts to find a component instance.
        /// </summary>
        /// <typeparam name="T">The component to find.</typeparam>
        /// <returns>The component instance if found, null otherwise.</returns>
        public static T FindComponent<T>() where T : Component
            => UnityEngine.Object.FindObjectOfType<T>();

        /// <summary>
        /// Gets all items of the specified type.
        /// </summary>
        /// <typeparam name="T">The item type to get.</typeparam>
        /// <returns>Found items.</returns>
        public static List<T> GetItems<T>() where T : BaseItem
            => Pickups.Where(x => x is T).Cast<T>().ToList();

        /// <summary>
        /// Tries to find the room that a <see cref="GameObject"/> is inside.
        /// </summary>
        /// <param name="objectInRoom">The <see cref="GameObject"/> inside the room.</param>
        /// <returns>The <see cref="Room"/> that the <see cref="GameObject"/> is located inside.</returns>
        public static Room FindRoom(GameObject objectInRoom)
        {
            var rooms = Rooms;

            Room room = null;

            if (!objectInRoom.CompareTag(PlayerTag))
            {
                room = objectInRoom.GetComponentInParent<Room>();
            }
            else
            {
                var ply = PlayersList.Get(objectInRoom);

                if (ply.Role == RoleType.Scp079)
                    room = FindRoom(ply.Hub.scp079PlayerScript.currentCamera.gameObject);
            }

            if (room == null)
            {
                Ray ray = new Ray(objectInRoom.transform.position, Vector3.down);

                if (Physics.RaycastNonAlloc(ray, cache, 10, 1 << 0, QueryTriggerInteraction.Ignore) == 1)
                    room = cache[0].collider.gameObject.GetComponentInParent<Room>();
            }

            if (room == null && rooms.Count != 0)
                room = rooms[rooms.Count - 1];

            return room;
        }

        /// <summary>
        /// Forces an overcharge resulting in lights off.
        /// </summary>
        /// <param name="time">The duration of the overcharge.</param>
        /// <param name="doors">Whether or not to lock doors too.</param>
        /// <param name="onlyZone">Restrict to this zone only.</param>
        public static void Overcharge(float time = 10f, bool doors = false, ZoneType? onlyZone = null)
        {
            if (doors)
            {
                foreach (var pair in InteractableCollider.AllInstances)
                {
                    if (!(pair.Key is BasicDoor basicDoor))
                        continue;

                    if (basicDoor == null)
                        continue;

                    if (basicDoor.RequiredPermissions.RequiredPermissions != KeycardPermissions.None)
                        continue;

                    if (!RoomIdentifier.RoomsByCoordinatess.TryGetValue(RoomIdUtils.PositionToCoords(basicDoor.transform.position), out RoomIdentifier rId))
                        continue;

                    if (onlyZone.HasValue && (ZoneType)rId.Zone != onlyZone.Value)
                        continue;

                    if (!onlyZone.HasValue && rId.Zone != FacilityZone.HeavyContainment)
                        continue;

                    basicDoor.NetworkTargetState = false;
                    basicDoor.ServerChangeLock(DoorLockReason.NoPower, true);

                    recontain?._lockedDoors?.Add(basicDoor);
                }
            }

            foreach (var ctrl in LightControllers)
            {
                if (!RoomIdentifier.RoomsByCoordinatess.TryGetValue(RoomIdUtils.PositionToCoords(ctrl.transform.position), out RoomIdentifier rId))
                    continue;

                if (onlyZone.HasValue && (ZoneType)rId.Zone != onlyZone.Value)
                    continue;

                if (!onlyZone.HasValue && rId.Zone != FacilityZone.HeavyContainment)
                    continue;

                if (!ctrl.TryGetComponent(out Scp079Interactable interact))
                    continue;

                if (interact.type != Scp079Interactable.InteractableType.LightController)
                    continue;

                ctrl.ServerFlickerLights(time);
            }
        }

        #region EventHandlers

        [EventHandler(typeof(MapGenerated))]
        internal static void OnMapGenerated()
        {
            doors.Clear();
            cams.Clear();
            rooms.Clear();
            chambers.Clear();
            generators.Clear();
            lockers.Clear();
            stations.Clear();
            tesla.Clear();
            windows.Clear();
            pocket?.exits?.Clear();
            targets.Clear();
            elevators.Clear();
            elevatorObjects.Clear();
            tantrums.Clear();

            BaseItem.items.Clear();
            BasePickup.pickups.Clear();

            Log.DebugFeature<Map>($"Cache cleared.");

            FillCache();
        }

        internal static void FillCache()
        {
            foreach (DoorVariant door in FindArray<DoorVariant>())
                doors.Add(new Door(door));

            foreach (Camera079 camera079 in Scp079PlayerScript.allCameras)
                cams.Add(new Camera(camera079));

            foreach (MapGeneration.Distributors.Locker locker in FindArray<MapGeneration.Distributors.Locker>())
                lockers.Add(new Locker(locker));

            foreach (Scp079Generator gen in FindArray<Scp079Generator>())
                generators.Add(new Generator(gen));

            foreach (WorkstationController station in WorkstationController.AllWorkstations)
                stations.Add(new Workstation(station));

            controller = UnityEngine.Object.FindObjectOfType<TeslaGateController>();

            foreach (TeslaGate gate in controller.TeslaGates)
                tesla.Add(new Tesla(gate));

            foreach (BreakableWindow window in FindArray<BreakableWindow>())
                windows.Add(new Window(window));

            foreach (ShootingTarget target in FindArray<ShootingTarget>())
                targets.Add(new Target(target));

            foreach (Lift lift in FindArray<Lift>())
                elevators.Add(new Elevator(lift));

            List<GameObject> roomObjects = ListPool<GameObject>.Shared.Rent();

            roomObjects.AddRange(GameObject.FindGameObjectsWithTag(RoomTag));

            if (roomObjects.Count == 0)
            {
                ListPool<GameObject>.Shared.Return(roomObjects);

                return;
            }

            var pocket = GameObject.Find(PocketDimensionTag);

            if (pocket != null)
                roomObjects.Add(pocket);

            var surface = GameObject.Find(SurfaceTag);

            if (surface != null)
                roomObjects.Add(surface);

            foreach (var roomObject in roomObjects)
            {
                var r = roomObject.AddComponent<Room>();

                if (r.Type == RoomType.Pocket)
                {
                    Map.pocket = new PocketDimension(r, r.Position);

                    Map.pocket.InternalRefreshPocketDimensionExits();
                }

                rooms.Add(r);
            }

            if (Map.pocket == null)
                Log.DebugFeature<Map>($"Failed to create a new Pocket Dimension object for the Map.Pocket property!");

            ListPool<GameObject>.Shared.Return(roomObjects);

            Log.DebugFeature<Map>($"Succesfully filled the map cache.");
        }

        [EventHandler(typeof(RoundWaiting))]
        internal static void OnWaitingForPlayers()
        {
            PlayersList.host = new Player(ReferenceHub.HostHub);

            icom = new Intercom(global::Intercom.host);
            scp914 = new Scp914(FindComponent<global::Scp914.Scp914Controller>());

            recontain = FindComponent<Recontainer079>();
            lsc = FindComponent<LureSubjectContainer>();
            femurBreaker = GameObject.FindGameObjectWithTag("FemurBreaker");

            if (PlayersList.host == null)
                Log.DebugFeature<Map>("Failed to set the PlayersList.host instance!");

            if (icom == null)
                Log.DebugFeature<Map>($"Failed to set the Map.icom instance!");

            if (scp914 == null)
                Log.DebugFeature<Map>($"Failed to set the Map.scp914 instance!");
        }

        [EventHandler(typeof(SpawnedTarget))]
        internal static void OnTargetSpawned(SpawnedTarget ev)
        {
            targets.Add(ev.Target);

            Log.DebugFeature<Map>($"Added a spawned target: {ev.Target.NetId}");
        }

        [EventHandler(typeof(SpawningTantrum))]
        internal static void OnSpawningTantrum(SpawningTantrum ev)
            => tantrums.Add(ev.Tantrum);

        // Gets called every time someone calls the Targets property.
        internal static void UpdateTargets()
        {
            var actual = ListPool<ShootingTarget>.Shared.Rent(FindArray<ShootingTarget>());

            Log.DebugFeature<Map>($"Updating targets.");

            int updatedTargets = 0;

            if (targets.Count != actual.Count)
            {
                Log.DebugFeature<Map>($"The current target list does not match the array of targets found in length.");

                targets.Clear();

                foreach (var act in actual)
                {
                    updatedTargets++;

                    targets.Add(new Target(act));
                }
            }
            else
            {
                for (int i = 0; i < actual.Count; i++)
                {
                    if (targets[i].NetId != actual[i].netId)
                    {
                        Log.DebugFeature<Map>($"OLDTARGET ({i}) [{targets[i].NetId}] DOES NOT MATCH NEWTARGET ({i}) [{actual[i].NetworkId}!");

                        targets[i] = new Target(actual[i], false);

                        updatedTargets++;
                    }
                }
            }

            ListPool<ShootingTarget>.Shared.Return(actual);

            Log.DebugFeature<Map>($"Updated {updatedTargets} target(s).");
        }

        #endregion
    }
}