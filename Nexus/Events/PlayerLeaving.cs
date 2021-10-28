using Nexus.Entities;
using Nexus.EventSystem;

namespace Nexus.Events
{
    /// <summary>
    /// Fires before a player leaves.
    /// </summary>
    public class PlayerLeaving : Event
    {
        /// <summary>
        /// Gets the leaving player.
        /// </summary>
        public Player Player { get; }

        public PlayerLeaving(Player player)
            => Player = player;
    }
}
