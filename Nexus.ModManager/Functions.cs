using System;
using System.IO;

using Nexus.Interfaces;
using Nexus.Events;
using Nexus.Attributes;
using Nexus.Enums;
using Nexus.EventSystem;
using Nexus.Events;

namespace Nexus.ModManager
{
    public static class Functions
    {
        [EventHandler(typeof(RoundWaiting))]
        public static void LogWarnings()
        {
            if (BuildInfo.BuildType != Enums.BuildType.Release)
                Log.Warn("Nexus", $"You are running an unstable {BuildInfo.BuildType} release! This version may contain a lot of bugs!");

            if (BuildInfo.IsForBeta && BuildInfo.BetaType == BetaBranch.Staging)
                Log.Warn("Nexus", $"This version ({BuildInfo.FullVersionString}) is for a staging branch! " +
                    $"Make sure to report every issue to the Nexus development team as Northwood does NOT provide any support for plugins on this branch!");

            if (BuildInfo.IsForBeta && BuildInfo.BetaType == BetaBranch.Public)
                Log.Warn("Nexus", $"This version ({BuildInfo.FullVersionString}) is for a public beta branch! This server version does NOT represent the full" +
                    $"product and may contain a lot of bugs!");
        }

        public static ModId GetModId(IModBase<IConfig> mod)
            => new ModId
            {
                AssemblyLocation = ModLoader.GetPath(mod.Assembly),
                AssemblyVersion = mod.Assembly.GetName().Version.ToString(),
                AssemblyName = Path.GetFileName(ModLoader.GetPath(mod.Assembly)),

                ModName = mod.Name,
                ModVersion = mod.Version.ToString()
            };

        /// <summary>
        /// Sends a INFO level message to the console.
        /// </summary>
        /// <param name="msg">The message to send.</param>
        public static void Info(object msg)
            => Log.Info("Nexus", msg);

        /// <summary>
        /// Forces a garbage collection.
        /// </summary>
        public static void CollectGarbage()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        /// <summary>
        /// An extension that will convert an <see cref="object"/> to the <typeparam name="T">type specified as the generic parameter.</typeparam>
        /// </summary>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted object if their type match, otherwise null.</returns>
        public static T As<T>(this object obj)
        {
            if (obj is T tValue)
                return tValue;

            return default;
        }

        /// <summary>
        /// Handles the ServerStopping event.
        /// </summary>
        public static void HandleServerQuit()
        {
            EventManager.Invoke(new ServerStopping());

            ModLoader.Exit(false);
        }
    }
}
