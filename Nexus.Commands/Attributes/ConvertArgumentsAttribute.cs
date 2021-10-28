using System;

namespace Nexus.Commands.Attributes
{
    /// <summary>
    /// Used to instruct the <see cref="CommandManager"/> to convert argument types when executing this command.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ConvertArgumentsAttribute : Attribute
    {
    }
}