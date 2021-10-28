using Nexus.EventSystem;
using Nexus.EventSystem.Attributes;

namespace Nexus.Events
{
    /// <summary>
    /// Fires before the server's process exits.
    /// </summary>
    [NoParam]
    public class ServerQuitting : Event
    {
    }
}
