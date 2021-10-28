using Nexus.Entities.Items.Base;

namespace Nexus.Entities.Items.Usable
{
    /// <summary>
    /// Represents the in-game medkit.
    /// </summary>
    public class Medkit : BaseConsumableItem
    {
        internal InventorySystem.Items.Usables.Medkit medkit;

        public Medkit(InventorySystem.Items.Usables.Medkit medkit, bool addToApi = false) : base(medkit, addToApi)
         => this.medkit = medkit;
    }
}
