using System.ComponentModel;

using Nexus.Interfaces;

namespace Nexus.ModManager.Configs.Nexus
{
    public class NexusConfiguration : IConfig
    {
        [Description("Turning this on will show debug messages that are important if something is not working.")]
        public bool DebugFeatures { get; set; } = false;

        [Description("Settings this config to true will allow loading of mismatched server versions.")]
        public bool LoadIncompatible { get; set; } = false;
    }
}
