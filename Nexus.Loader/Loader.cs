using System;
using System.Net;
using System.IO;
using System.Reflection;

using UnityEngine;

namespace Nexus.Loader
{
    public sealed class Loader
    {
        public const string LatestRelease = "https://github.com/ondax/Nexus/releases/latest/download/";

        public static string SCPSL_Data => Application.dataPath;
        public static string Home => Directory.GetParent(SCPSL_Data).FullName;

        public static string Folder => $"{Home}/Nexus";
        public static string ThisFolder => $"{Folder}/{(ServerStatic.ServerPortSet ? ServerStatic.ServerPort : 7777)}";
        public static string Dependencies => $"{ThisFolder}/Dependencies";
        public static string Mods => $"{ThisFolder}/Mods";
        public static string Translations => $"{ThisFolder}/Translations";
        public static string Configs => $"{ThisFolder}/Configuration";
        public static string ModConfigs => $"{Configs}/Mods";
        public static string ModTranslations => $"{Translations}/Mods";
        public static string Nexus => ThisFolder;

        public static string NexusFile => $"{Nexus}/Nexus.dll";
        public static string NexusModManager => $"{Nexus}/Nexus.ModManager.dll";
        public static string NexusPatches => $"{Nexus}/Nexus.Patches.dll";
        public static string NexusCommands => $"{Nexus}/Nexus.Commands.dll";
        public static string NexusTranslations => $"{Nexus}/Nexus.Translations.dll";

        public static string YamlDotNet => $"{Dependencies}/YamlDotNet.dll";
        public static string ComponentModel => $"{Dependencies}/System.ComponentModel.Annotations.dll";
        public static string Harmony => $"{Dependencies}/0Harmony.dll";

        public static void Check()
        {
            try
            {
                if (!Directory.Exists(Folder))
                    Directory.CreateDirectory(Folder);

                if (!Directory.Exists(ThisFolder))
                    Directory.CreateDirectory(ThisFolder);

                if (!Directory.Exists(Dependencies))
                    Directory.CreateDirectory(Dependencies);

                if (!Directory.Exists(Mods))
                    Directory.CreateDirectory(Mods);

                if (!Directory.Exists(Translations))
                    Directory.CreateDirectory(Translations);

                if (!Directory.Exists(Configs))
                    Directory.CreateDirectory(Configs);

                if (!Directory.Exists(Nexus))
                    Directory.CreateDirectory(Nexus);

                if (!Directory.Exists(ModConfigs))
                    Directory.CreateDirectory(ModConfigs);

                if (!Directory.Exists(ModTranslations))
                    Directory.CreateDirectory(ModTranslations);
            }
            catch (Exception ex)
            {
                ServerConsole.AddLog(ex.ToString(), ConsoleColor.DarkRed);
            }
        }

        public static void Log(object message)
                => ServerConsole.AddLog($"[INFO] [Nexus] {message}", ConsoleColor.Red);

        public static void Download(string link, string destination)
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(link, destination);
            }
        }

        public static void VerifyFile(string path, string link)
        {
            if (!File.Exists(path))
                Download(link, path);
        }

        public static void Load()
        {
            try
            {
                string nexusLink = $"{LatestRelease}Nexus.dll";
                string nexusCommandsLink = $"{LatestRelease}Nexus.Commands.dll";
                string nexusModManagerLink = $"{LatestRelease}Nexus.ModManager.dll";
                string nexusPatchesLink = $"{LatestRelease}Nexus.Patches.dll";
                string nexusTranslationsLink = $"{LatestRelease}Nexus.Translations.dll";
                string yamlLink = $"{LatestRelease}YamlDotNet.dll";
                string componentModelLink = $"{LatestRelease}System.ComponentModel.Annotations.dll";
                string harmonyLink = $"{LatestRelease}0Harmony.dll";

                Check();

                VerifyFile(NexusFile, nexusLink);
                VerifyFile(NexusPatches, nexusPatchesLink);
                VerifyFile(NexusModManager, nexusModManagerLink);
                VerifyFile(NexusCommands, nexusCommandsLink);
                VerifyFile(NexusTranslations, nexusTranslationsLink);
                VerifyFile(YamlDotNet, yamlLink);
                VerifyFile(ComponentModel, componentModelLink);
                VerifyFile(Harmony, harmonyLink);

                Assembly harmonyA = Assembly.LoadFrom(Harmony);
                Assembly componentModelA = Assembly.LoadFrom(ComponentModel);
                Assembly yamlDotNetA = Assembly.LoadFrom(YamlDotNet);
                Assembly nexusTransA = Assembly.LoadFrom(NexusTranslations);
                Assembly nexusCommandsA = Assembly.LoadFrom(NexusCommands);
                Assembly nexusA = Assembly.LoadFrom(NexusFile);
                Assembly nexusModManagerA = Assembly.LoadFrom(NexusModManager);
                Assembly nexusPatchesA = Assembly.LoadFrom(NexusPatches);

                nexusModManagerA?.GetType("Nexus.ModManager.ModLoader")?.GetMethod("Reload")?.Invoke(null,
                    new object[]
                    {
                        new Assembly[]
                        {
                            harmonyA,
                            componentModelA,
                            yamlDotNetA,
                            nexusTransA,
                            nexusCommandsA,
                            nexusA,
                            nexusModManagerA,
                            nexusPatchesA
                        }
                    });
            }
            catch (Exception e)
            {
                Log(e);

                return;
            }
        }
    }
}