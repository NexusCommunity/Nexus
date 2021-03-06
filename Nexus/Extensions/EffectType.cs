using System;

using Nexus.Enums;

using CustomPlayerEffects;

namespace Nexus.Extensions
{
    public static class EffectTypeExtensions
    {
        public static Type Type(this EffectType effect)
        {
            switch (effect)
            {
                case EffectType.Amnesia: return typeof(Amnesia);
                case EffectType.Asphyxiated: return typeof(Asphyxiated);
                case EffectType.Bleeding: return typeof(Bleeding);
                case EffectType.Blinded: return typeof(Blinded);
                case EffectType.Burned: return typeof(Burned);
                case EffectType.Concussed: return typeof(Concussed);
                case EffectType.Corroding: return typeof(Corroding);
                case EffectType.Deafened: return typeof(Deafened);
                case EffectType.Decontaminating: return typeof(Decontaminating);
                case EffectType.Disabled: return typeof(Disabled);
                case EffectType.Ensnared: return typeof(Ensnared);
                case EffectType.Exhausted: return typeof(Exhausted);
                case EffectType.Flashed: return typeof(Flashed);
                case EffectType.Hemorrhage: return typeof(Hemorrhage);
                case EffectType.Invigorated: return typeof(Invigorated);
                case EffectType.Panic: return typeof(Panic);
                case EffectType.Poisoned: return typeof(Poisoned);
                case EffectType.Scp207: return typeof(Scp207);
                case EffectType.SinkHole: return typeof(SinkHole);
                case EffectType.Visuals939: return typeof(Visuals939);
            }

            throw new InvalidOperationException("Invalid effect enum provided");
        }
    }
}