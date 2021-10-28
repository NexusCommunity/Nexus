﻿using System;

namespace Nexus
{
    /// <summary>
    /// A set of tools to handle logging more easily.
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// Adds an informational message to the server output.
        /// </summary>
        /// <typeparam name="T">The type name that will be used as the source.</typeparam>
        /// <param name="message">The message.</param>
        public static void FullInfo<T>(object message)
            => Info(typeof(T).FullName, message);

        /// <summary>
        /// Adds an informational message to the server output.
        /// </summary>
        /// <typeparam name="T">The type name that will be used as the source.</typeparam>
        /// <param name="message">The message.</param>
        public static void Info<T>(object message)
            => Info(typeof(T).Name, message);

        /// <summary>
        /// Adds an informational message to the server output.
        /// </summary>
        /// <param name="source">The source of this message.</param>
        /// <param name="message">The message.</param>
        public static void Info(object source, object message)
            => Add("INFO", source, message, ConsoleColor.Cyan);

        /// <summary>
        /// Adds a debug message to the server output.
        /// </summary>
        /// <typeparam name="T">The type name that will be used as the source.</typeparam>
        /// <param name="message">The messsage.</param>
        public static void FullDebug<T>(object message)
            => Debug(typeof(T).FullName, message);

        /// <summary>
        /// Adds a debug message to the server output.
        /// </summary>
        /// <typeparam name="T">The type name that will be used as the source.</typeparam>
        /// <param name="message">The messsage.</param>
        public static void Debug<T>(object message)
            => Debug(typeof(T).Name, message);

        /// <summary>
        /// Adds a debug message to the server output.
        /// </summary>
        /// <param name="source">The source of this message.</param>
        /// <param name="message">The message.</param>
        public static void Debug(object source, object message)
            => Add("DEBUG", source, message, ConsoleColor.White);

        /// <summary>
        /// Adds a warning to the server output.
        /// </summary>
        /// <typeparam name="T">The type name that will be used as the source.</typeparam>
        /// <param name="message">The message.</param>
        public static void FullWarn<T>(object message)
            => Warn(typeof(T).FullName, message);

        /// <summary>
        /// Adds a warning to the server output.
        /// </summary>
        /// <typeparam name="T">The type name that will be used as the source.</typeparam>
        /// <param name="message">The message.</param>
        public static void Warn<T>(object message)
            => Warn(typeof(T).Name, message);

        /// <summary>
        /// Adds a warning to the server output.
        /// </summary>
        /// <param name="source">The source of this message-</param>
        /// <param name="message">The message.</param>
        public static void Warn(object source, object message)
            => Add("WARN", source, message, ConsoleColor.Yellow);

        /// <summary>
        /// Adds an error to the server output.
        /// </summary>
        /// <typeparam name="T">The type name that will be used as the source.</typeparam>
        /// <param name="message">The message.</param>
        public static void FullError<T>(object message)
            => Error(typeof(T).FullName, message);

        /// <summary>
        /// Adds an error to the server output.
        /// </summary>
        /// <typeparam name="T">The type name that will be used as the source.</typeparam>
        /// <param name="message">The message.</param>
        public static void Error<T>(object message)
            => Error(typeof(T).Name, message);

        /// <summary>
        /// Adds an error to the server output.
        /// </summary>
        /// <param name="source">The source of this message.</param>
        /// <param name="message">The message.</param>
        public static void Error(object source, object message)
            => Add("ERROR", source, message, ConsoleColor.Red);

        /// <summary>
        /// Adds an exception to the server output.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public static void Exception(Exception exception)
            => Add("ERROR", "Nexus", exception, ConsoleColor.DarkRed);

        internal static void NexusInfo(object message)
            => Add("INFO", "Nexus", message, ConsoleColor.Cyan);

        internal static void NexusWarn(object message)
            => Add("WARN", "Nexus", message, ConsoleColor.DarkYellow);

        internal static void NexusDebug(object message)
            => Add("DEBUG", "Nexus", message, ConsoleColor.Magenta);

        internal static void NexusError(object message)
            => Add("ERROR", "Nexus", message, ConsoleColor.DarkRed);

        internal static void NexusDev(object message)
        {
            if (BuildInfo.BuildType != Enums.BuildType.Release)
                Add("DEV", message, ConsoleColor.Green);
        }

        /// <summary>
        /// Sends a debug message if <see cref="ModManager.Configs.ServerConfiguration.DebugFeatures"/> is set to true.
        /// </summary>
        /// <typeparam name="TFeature">The feature used as a source.</typeparam>
        /// <param name="message">The message.</param>
        public static void DebugFeature<TFeature>(object message)
        {
            if (!ConfigHolder.Server.DebugFeatures)
                return;

            Debug<TFeature>(message);
        }

        /// <summary>
        /// Adds a message to the server output.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="message">The message.</param>
        /// <param name="color">The color of this message.</param>
        public static void Add(object tag, object message, ConsoleColor color = ConsoleColor.Gray)
            => Add(tag, "Nexus", message, color);

        /// <summary>
        /// Adds a message to the server output.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="source">The source of this message.</param>
        /// <param name="message">The message.</param>
        /// <param name="color">The color of this message.</param>
        public static void Add(object tag, object source, object message, ConsoleColor color = ConsoleColor.Gray)
            => Add($"[{tag}] [{source}] {message}", color);

        /// <summary>
        /// Adds a message to the server output.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <param name="color">The color of this message.</param>
        public static void Add(object message, ConsoleColor color = ConsoleColor.Gray)
            => ServerConsole.AddLog(message.ToString(), color);
    }
}