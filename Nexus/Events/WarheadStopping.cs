using Nexus.Entities;
using Nexus.EventSystem;

namespace Nexus.Events
{
    /// <summary>
    /// Fires when the warhead detonation tries to stop.
    /// </summary>
    public class WarheadStopping : BoolEvent
    {
        /// <summary>
        /// Gets the player that stopped the detonation. <b>May be null.</b>
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the remaining time to detonation.
        /// </summary>
        public int RemainingTime { get; }

        public WarheadStopping(Player player, int time, bool allow)
        {
            Player = player;
            RemainingTime = time;
            IsAllowed = allow;
        }
    }
}
