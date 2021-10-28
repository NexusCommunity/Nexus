using Nexus.Entities;
using Nexus.EventSystem;

namespace Nexus.Events
{
    /// <summary>
    /// Fires when a player changes the outsite warhead panel lever status.
    /// </summary>
    public class ChangingLeverStatus : BoolEvent
    {
        /// <summary>
        /// Gets the player trying to change the lever status.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the lever's current status.
        /// </summary>
        public bool Current { get; }

        /// <summary>
        /// Gets or sets the lever's new status.
        /// </summary>
        public bool Target { get; set; }

        public ChangingLeverStatus(Player player, bool curr, bool tar, bool allow)
        {
            Player = player;
            Current = curr;
            Target = tar;
            IsAllowed = allow;
        }
    }
}
