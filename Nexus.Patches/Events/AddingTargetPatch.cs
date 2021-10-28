using Nexus.Entities;
using Nexus.Enums;
using Nexus.Events;
using Nexus.EventSystem;

using System;
using PlayableScps;
using PlayableScps.Messages;
using UnityEngine;

using HarmonyLib;

namespace Nexus.Patches.Events
{
    [HarmonyPatch(typeof(Scp096), nameof(Scp096.AddTarget))]
    public class AddingTargetPatch
    {
        public static bool Prefix(Scp096 __instance, GameObject target)
        {
            try
            {
                Player player = PlayersList.Get(target);

                if (!__instance.CanReceiveTargets || player == null || __instance._targets.Contains(player.Hub))
                    return false;

                AddingTargetScp096 ev = EventManager.Invoke(new AddingTargetScp096(PlayersList.Get(__instance.Hub), player, __instance, true));

                if (!ev.IsAllowed)
                    return false;

                if (!__instance._targets.IsEmpty() || __instance.PlayerState == Scp096PlayerState.Docile || __instance.Enraging)
                    __instance.AddReset();

                if (__instance._targets.IsEmpty())
                    ev.Player.Connection.Send(new Scp096ToTargetMessage(__instance.Hub));

                __instance._targets.Add(player.Hub);

                player.Connection.Send(new Scp096ToTargetMessage(player.Hub));

                return false;
            }
            catch (Exception e)
            {
                Manager.Exc<AddingTargetPatch>(e);

                return true;
            }
        }
    }
}
