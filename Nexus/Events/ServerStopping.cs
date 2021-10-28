using Nexus.EventSystem.Attributes;
using Nexus.EventSystem;

namespace Nexus.Events
{
    /// <summary>
    /// Fires before the server process quits. This event may not fire every time.
    /// </summary>
    [NoParam]
    public class ServerStopping : Event
    {
    }
}
