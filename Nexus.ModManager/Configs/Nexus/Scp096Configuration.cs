using System.ComponentModel;
using System.Collections.Generic;

using Nexus.Interfaces;

namespace Nexus.ModManager.Configs.Nexus
{
    public class Scp096Configuration : IConfig
    {
        public List<RoleType> DisallowedRoles { get; set; } = new List<RoleType>() { RoleType.Tutorial };

        public bool DisableBypass { get; set; } = false;
        public bool DisableGodMode { get; set; } = false;
    }
}
