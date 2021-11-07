using System.ComponentModel;

using Nexus.Interfaces;

namespace Nexus.ModManager.Configs.Nexus
{
    public class Scp914Configuration : IConfig
    {
        public float KnobChangeCooldown { get; set; } = 1f;

        public bool PlaySoundOnKnobChange { get; set; } = true;

        public bool PlaySoundOnActivation { get; set; } = true;
    }
}
