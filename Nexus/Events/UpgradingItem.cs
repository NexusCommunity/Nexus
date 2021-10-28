using Nexus.EventSystem;
using Nexus.Entities.Items.Base;

namespace Nexus.Events
{
    /// <summary>
    /// Fires when SCP-914 tries to upgrade an item.
    /// </summary>
    public class UpgradingItem : BoolEvent
    {
        /// <summary>
        /// Gets the item that is being upgraded.
        /// </summary>
        public BaseItem Item { get; }

        /// <summary>
        /// Gets or sets the new <see cref="ItemType"/> for this item.
        /// </summary>
        public ItemType TargetId { get; set; }

        public UpgradingItem(BaseItem item, ItemType targetId, bool allow)
        {
            Item = item;
            TargetId = targetId;
            IsAllowed = allow;
        }
    }
}