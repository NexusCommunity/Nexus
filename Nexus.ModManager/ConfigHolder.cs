using Nexus.ModManager.Configs;
using Nexus.ModManager;

/// <summary>
/// Used to manage Nexus configuration.
/// </summary>
public static class ConfigHolder
{
    /// <summary>
    /// Gets the <see cref="ServerConfiguration"/> instance.
    /// </summary>
    public static readonly ServerConfiguration Server = new ServerConfiguration();

    /// <summary>
    /// Reloads all Nexus configs.
    /// </summary>
    public static void Reload()
    {
        ConfigManager.Reload(Server);
    }
}
