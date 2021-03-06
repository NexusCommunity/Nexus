using Nexus.Entities.Pickups.Base;

namespace Nexus.Entities.Pickups
{
    /// <summary>
    /// Represents the in-game keycard pickup.
    /// </summary>
    public class KeycardPickup : BasePickup
    {
        internal InventorySystem.Items.Keycards.KeycardPickup card;

        public KeycardPickup(InventorySystem.Items.Keycards.KeycardPickup card, bool addToApi = false) : base(card, addToApi)
            => this.card = card;
    }
}
