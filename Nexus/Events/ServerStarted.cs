using Nexus.EventSystem;
using Nexus.EventSystem.Attributes;

namespace Nexus.Events
{
    /// <summary>
    /// Fires when the server fully starts.
    /// </summary>
    [NoParam]
    public class ServerStarted : Event
    {
    }
}
