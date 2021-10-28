using Nexus.Entities;
using Nexus.EventSystem;

namespace Nexus.Events
{
    /// <summary>
    /// Fires when a generator activates.
    /// </summary>
    public class GeneratorActivated : BoolEvent
    {
        /// <summary>
        /// Gets the generator that activated.
        /// </summary>
        public Generator Generator { get; }

        public GeneratorActivated(Generator gen, bool allow)
        {
            Generator = gen;
            IsAllowed = allow;
        }
    }
}
