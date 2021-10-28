using Nexus.Entities;
using Nexus.EventSystem;

using System.Collections.Generic;

namespace Nexus.Events
{
    /// <summary>
    /// Fires when SCP-173 forces players to blink.
    /// </summary>
    public class Blinking : Event
    {
        /// <summary>
        /// Gets the player playing as SCP-173.
        /// </summary>
        public Player Scp { get; }

        /// <summary>
        /// Gets the blinking players.
        /// </summary>
        public HashSet<Player> Targets { get; }

        public Blinking(Player scp, HashSet<Player> targets)
        {
            Scp = scp;
            Targets = targets;
        }
    }
}
