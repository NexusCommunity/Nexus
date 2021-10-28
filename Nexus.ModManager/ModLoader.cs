﻿using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

using Nexus.Mod;
using Nexus.Interfaces;
using Nexus.Translations;
using Nexus.Commands;
using Nexus.EventSystem;
using Nexus.Patches;
using Nexus.ModManager.Configs;
using Nexus.ModManager.Configs.Converters;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization.NodeDeserializers;

namespace Nexus.ModManager
{
    public static class ModLoader
    {
        internal static Dictionary<string, IModBase<IConfig>> mods;
        internal static Dictionary<string, Assembly> deps;
        internal static Dictionary<string, ModId> ids;
        internal static Dictionary<string, string> paths;

        internal static string defaultCommandDesc;
        internal static bool commandSet;
        internal static bool eventRegistered;

        static ModLoader()
        {
            mods = new Dictionary<string, IModBase<IConfig>>();
            deps = new Dictionary<string, Assembly>();
            ids = new Dictionary<string, ModId>();
            paths = new Dictionary<string, string>();

            Patcher = new Manager();
        }

        /// <summary>
        /// Gets a <see cref="IReadOnlyDictionary{TKey, TValue}"/> of all active mods.
        /// </summary>
        public static IReadOnlyDictionary<string, IModBase<IConfig>> ActiveMods => mods;

        /// <summary>
        /// Gets a <see cref="IReadOnlyDictionary{TKey, TValue}"/> of dependencies.
        /// </summary>
        public static IReadOnlyDictionary<string, Assembly> Dependencies => deps;

        /// <summary>
        /// Gets the patcher instance.
        /// </summary>
        public static Manager Patcher { get; }

        /// <summary>
        /// Gets the serializer for configs and translations.
        /// </summary>
        public static ISerializer Serializer { get; } = new SerializerBuilder()
            .WithTypeConverter(new VectorsConverter())
            .WithTypeInspector(inner => new CommentGatheringTypeInspector(inner))
            .WithEmissionPhaseObjectGraphVisitor(args => new CommentsObjectGraphVisitor(args.InnerVisitor))
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .IgnoreFields()
            .Build();

        /// <summary>
        /// Gets the deserializer for configs and translations.
        /// </summary>
        public static IDeserializer Deserializer { get; } = new DeserializerBuilder()
            .WithTypeConverter(new VectorsConverter())
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .WithNodeDeserializer(inner => new ValidatingNodeDeserializer(inner), deserializer => deserializer.InsteadOf<ObjectNodeDeserializer>())
            .IgnoreFields()
            .IgnoreUnmatchedProperties()
            .Build();

        /// <summary>
        /// Reloads all plugins and dependencies.
        /// </summary>
        /// <param name="depArray">Pre-loaded dependencies by the loader.</param>
        public static void Reload(Assembly[] depArray = null)
        {
            if (depArray != null && depArray.Length > 0)
            {
                foreach (Assembly assembly in depArray)
                {
                    deps.Add(assembly.Location, assembly);
                }
            }

            UnityEngine.Application.quitting += Functions.HandleServerQuit;

            ConfigHolder.Reload();

            if (!BuildInfo.IsCompatible())
            {
                Log.Error("Nexus", $"Version mismatch! This version of Nexus cannot be loaded with this server version! " +
                    $"(Expected: {BuildInfo.ExpectedServerVersion} | Server: {GameCore.Version.VersionString})");
                Log.Error("Nexus", $"You can set the \"load_incompatible\" config to \"true\" if you wan't to load this version, but this is not recommended!");

                return;
            }

            Functions.Info($"Loading version {BuildInfo.FullVersionString} ...");

            foreach (string depFile in Directory.GetFiles(Loader.Loader.Dependencies, "*.dll"))
            {
                if (deps.ContainsKey(depFile))
                    continue;

                Assembly depAssembly = Assembly.Load(File.ReadAllBytes(depFile));

                if (depAssembly != null)
                {
                    deps.Add(depFile, depAssembly);

                    Functions.Info($"Loaded dependency: {depAssembly.GetName().Name}@{depAssembly.GetName().Version}");
                }
            }

            Patcher.PatchPatches();

            var mods = ModLoader.mods;
            var ids = ModLoader.ids;

            foreach (string file in Directory.GetFiles(Loader.Loader.Mods, "*.dll"))
            {
                if (mods.TryGetValue(file, out IModBase<IConfig> value))
                {
                    ModId newId = Functions.GetModId(value);

                    if (newId.HasChanged(ids[file]))
                    {
                        CommandManager.UnregisterCommands(value.Assembly);
                        EventManager.Unregister(value.Assembly);

                        value.Translations.Clear();

                        value.Disable();

                        ModLoader.mods.Remove(file);
                        ModLoader.ids.Remove(file);

                        Functions.CollectGarbage();

                        Assembly assembly = Assembly.Load(File.ReadAllBytes(file));

                        AssemblyName name = assembly.GetName();

                        if (!paths.ContainsKey(name.Name))
                            paths.Add(name.Name, Path.GetFullPath(file));

                        if (assembly != null)
                        {
                            IModBase<IConfig> mod = Create(assembly);

                            if (mod != null)
                            {
                                ModLoader.mods.Add(file, mod);
                                ModLoader.ids.Add(file, newId);

                                TranslationManager.FindTranslations(mod);

                                Functions.Info($"Loaded mod: {mod.Name}@{mod.Version} by {mod.Author}!");

                                ConfigManager.Reload(mod);
                                TranslationManager.Reload(mod);
                            }
                        }

                        continue;
                    }

                    ConfigManager.Reload(value);

                    TranslationManager.Reload(value);

                    value.Reload();

                    Functions.Info($"Reloaded mod: {value.Name}");
                }
                else
                {
                    Assembly assembly = Assembly.Load(File.ReadAllBytes(file));

                    AssemblyName name = assembly.GetName();

                    if (!paths.ContainsKey(name.Name))
                        paths.Add(name.Name, Path.GetFullPath(file));

                    if (assembly != null)
                    {
                        IModBase<IConfig> mod = Create(assembly);

                        if (mod != null)
                        {
                            ModLoader.mods.Add(file, mod);
                            ModLoader.ids.Add(file, Functions.GetModId(mod));

                            TranslationManager.FindTranslations(mod);

                            Functions.Info($"Loaded mod: {mod.Name}@{mod.Version} by {mod.Author}!");

                            ConfigManager.Reload(mod);
                            TranslationManager.Reload(mod);
                        }
                    }
                }
            }

            Enable();

            // set the modded flag so NW doesnt come to your house
            if (!CustomNetworkManager.Modded)
                CustomNetworkManager.Modded = true;

            if (!commandSet)
            {
                defaultCommandDesc = CommandSystem.Commands.Shared.BuildInfoCommand.ModDescription;

                CommandSystem.Commands.Shared.BuildInfoCommand.ModDescription += $"\nFramework: Nexus {BuildInfo.FullVersionString} - {ActiveMods.Count} mod(s) loaded - {Dependencies.Count} dependencies loaded.";

                commandSet = true;
            }

            if (!eventRegistered)
            {
                EventManager.Register(typeof(Functions));

                eventRegistered = true;
            }
        }

        public static string GetPath(Assembly assembly)
            => paths.TryGetValue(assembly.GetName().Name, out string path) ? path : string.Empty;

        public static void Enable()
        {
            foreach (IModBase<IConfig> mod in mods.Values)
            {
                try
                {
                    mod.Enable();

                    Functions.Info($"Enabled mod: {mod.Name}@{mod.Version} by {mod.Author}!");
                }
                catch (Exception e)
                {
                    Log.Exception(e);
                }
            }
        }

        /// <summary>
        /// Disables all mods and unpatches all patches.
        /// </summary>
        /// <param name="unpatch"></param>
        public static void Disable(bool unpatch)
        {
            foreach (IModBase<IConfig> mod in mods.Values)
            {
                try
                {
                    mod.Disable();

                    Functions.Info($"Disabled mod: {mod.Name}");
                }
                catch (Exception e)
                {
                    Log.Exception(e);
                }
            }

            if (unpatch)
            {
                Patcher.UnpatchPatches();
            }
        }

        /// <summary>
        /// Disables all plugins and exits the server process.
        /// </summary>
        public static void Exit(bool killServerProcess = false)
        {
            Disable(true);

            mods.Clear();
            deps.Clear();
            ids.Clear();

            commandSet = false;
            eventRegistered = false;

            CommandSystem.Commands.Shared.BuildInfoCommand.ModDescription = defaultCommandDesc;

            EventManager.Unregister(typeof(Functions));

            Functions.CollectGarbage();

            if (killServerProcess)
                UnityEngine.Application.Quit();
        }

        internal static IModBase<IConfig> Create(Assembly assembly)
        {
            try
            {
                Log.Debug("Nexus", $"Loading a mod assembly: {assembly.GetName().FullName}");

                foreach (Type type in assembly.GetTypes().Where(type => !type.IsAbstract && !type.IsInterface))
                {
                    if (!type.BaseType.IsGenericType || (type.BaseType.GetGenericTypeDefinition() != typeof(ModBase<>)))
                        continue;

                    IModBase<IConfig> mod = null;

                    var constructor = type.GetConstructor(Type.EmptyTypes);

                    if (constructor != null)
                    {
                        mod = constructor.Invoke(null) as IModBase<IConfig>;
                    }
                    else
                    {
                        var value = Array.Find(type.GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public), 
                            property => property.PropertyType == type)?.GetValue(null);

                        if (value != null)
                            mod = value as IModBase<IConfig>;
                    }

                    if (mod == null)
                    {
                        Log.Error("Nexus", $"{type.FullName} is a valid mod, but it cannot be instantiated!");

                        continue;
                    }

                    return mod;
                }
            }
            catch (Exception e)
            {
                Log.Error("Nexus", $"Error while initializing mod {assembly.GetName().Name} (at {GetPath(assembly)})!");
                Log.Error("Nexus", e);
            }

            return null;
        }
    }
}