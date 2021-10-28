using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nexus.Interfaces;

namespace Nexus.ModManager.Configs
{
    public class ServerConfiguration : IConfig
    {
        [Description("Turning this on will print a message to the console everytime someone gets banned.")]
        public bool LogBansToConsole { get; set; } = false;

        [Description("Turning this on will show debug messages that are important if something is not working.")]
        public bool DebugFeatures { get; set; } = false;
    }
}