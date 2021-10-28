using Nexus.EventSystem;
using Nexus.EventSystem.Attributes;

namespace Nexus.Events
{
    /// <summary>
    /// Fires when the server starts waiting for players.
    /// </summary>
    [NoParam]
    public class RoundWaiting : Event
    {
    }
}
