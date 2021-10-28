using Nexus.Entities;
using Nexus.Enums;
using Nexus.Events;
using Nexus.EventSystem;
using Scp914;

using HarmonyLib;

using System;

namespace Nexus.Patches.Events
{
    [HarmonyPatch(typeof(Scp914Controller), nameof(Scp914Controller.ServerInteract))]
    public class ActivatingScp914Patch
    {
        public static bool Prefix(Scp914Controller __instance, ReferenceHub ply, byte colliderId)
        {
            try
            {
				if (__instance._remainingCooldown > 0f)
					return false;

				Player player = PlayersList.Get(ply);

				if (player == null)
					return true;

				if (colliderId == 0)
				{
					__instance._remainingCooldown = __instance._knobChangeCooldown;

					Scp914KnobSetting scp914KnobSetting = __instance._knobSetting + 1;

					if (scp914KnobSetting > Scp914KnobSetting.VeryFine)
						scp914KnobSetting = Scp914KnobSetting.Rough;

					ChangingKnob ev = EventManager.Invoke(new ChangingKnob(player, (KnobSetting)__instance.Network_knobSetting, (KnobSetting)scp914KnobSetting, true));

					if (!ev.IsAllowed)
						return false;

					__instance.Network_knobSetting = (Scp914KnobSetting)ev.New;

					__instance.RpcPlaySound(0);

					return false;
				}

				if (colliderId != 1)
					return false;

				ActivatingScp914 aEv = EventManager.Invoke(new ActivatingScp914(player, __instance, true));

				if (!aEv.IsAllowed)
					return false;

				__instance._remainingCooldown = __instance._totalSequenceTime;
				__instance._isUpgrading = true;
				__instance._itemsAlreadyUpgraded = false;

				__instance.RpcPlaySound(1);

				return false;
            }
            catch (Exception e)
            {
                Manager.Exc<ActivatingScp914Patch>(e);

                return true;
            }
        }
    }
}
