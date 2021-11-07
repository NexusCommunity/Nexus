using System.ComponentModel;

using Nexus.Interfaces;

namespace Nexus.ModManager.Configs.Nexus
{
    public class ServerConfiguration : IConfig
    {
        [Description("Turning this on will print a message to the console everytime someone gets banned.")]
        public bool LogBansToConsole { get; set; } = false;
    }
}