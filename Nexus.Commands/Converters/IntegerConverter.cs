using Nexus.Commands.Interfaces;

namespace Nexus.Commands.Converters
{
    /// <summary>
    /// A default <see cref="int"/> converter.
    /// </summary>
    public class IntegerConverter : IConverter
    {
        public object Convert(string value) 
            => int.TryParse(value, out int val) ? val : 0;
    }
}