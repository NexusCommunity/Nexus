using Nexus.Commands.Interfaces;
using Nexus.Entities;

namespace Nexus.Commands.Converters
{
    /// <summary>
    /// A default <see cref="Player"/> converter.
    /// </summary>
    public class PlayerConverter : IConverter
    {
        public object Convert(string value)
            => PlayersList.Get(value);
    }
}