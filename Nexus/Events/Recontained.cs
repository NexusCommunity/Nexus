using Nexus.Entities;
using Nexus.EventSystem;

namespace Nexus.Events
{
    public class Recontained : Event
    {
        public Recontained(Player target)
        {
            Target = target;
        }

        /// <summary>
        /// Gets the player that previously controlled SCP-079.
        /// </summary>
        public Player Target { get; }
    }
}
