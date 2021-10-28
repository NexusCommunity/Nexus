using Nexus.Entities;
using Nexus.EventSystem;

namespace Nexus.Events
{
    /// <summary>
    /// Fires before a player's handcuffs get removed.
    /// </summary>
    public class RemovingHandcuffs : BoolEvent
    {
        /// <summary>
        /// Gets the player who is removing these handcuffs.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the player that has these handcuffs.
        /// </summary>
        public Player Target { get; }

        public RemovingHandcuffs(Player player, Player target, bool allow)
        {
            Player = player;
            Target = target;
            IsAllowed = allow;
        }
    }
}
