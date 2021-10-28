using Nexus.Entities;
using Nexus.EventSystem;

namespace Nexus.Events
{
    /// <summary>
    /// Fires when SCP-096 starts calming down.
    /// </summary>
    public class CalmingDown : BoolEvent
    {
        /// <summary>
        /// Gets the player playing as SCP-096.
        /// </summary>
        public Player Scp { get; }

        public CalmingDown(Player scp, bool allow)
        {
            Scp = scp;

            IsAllowed = allow;
        }
    }
}
