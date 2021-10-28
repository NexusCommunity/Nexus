using Nexus.Entities;
using Nexus.Entities.Items.Base;

using Nexus.EventSystem;

namespace Nexus.Events
{
    /// <summary>
    /// Fires before a player drops an item.
    /// </summary>
    public class DroppingItem : BoolEvent
    {
        /// <summary>
        /// Gets the player who is dropping the item.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets or sets the item being dropped.
        /// </summary>
        public BaseItem Item { get; set; }

        public DroppingItem(Player player, BaseItem item, bool allow)
        {
            Player = player;
            Item = item;
            IsAllowed = allow;
        }
    }
}
