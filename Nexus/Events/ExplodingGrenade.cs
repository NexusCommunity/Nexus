using Nexus.Entities;
using Nexus.Entities.Pickups.Base;
using Nexus.EventSystem;

using System.Collections.Generic;

namespace Nexus.Events
{
    /// <summary>
    /// Fires before a grenade explodes.
    /// </summary>
    public class ExplodingGrenade : BoolEvent
    {
        /// <summary>
        /// Gets the player that threw this grenade.
        /// </summary>
        public Player Thrower { get; }

        /// <summary>
        /// Gets the grenade that is exploding.
        /// </summary>
        public BaseGrenade Grenade { get; }

        /// <summary>
        /// Gets the dictionary containing all targets <i>(where keys are the targets and values the damage this grenade will deal to them)</i>.
        /// </summary>
        public Dictionary<Player, float> Targets { get; }
    }
}
