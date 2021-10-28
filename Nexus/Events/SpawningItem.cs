using Nexus.Entities.Pickups.Base;
using Nexus.Entities;
using Nexus.EventSystem;

namespace Nexus.Events
{
    /// <summary>
    /// Fires before an item spawns.
    /// </summary>
    public class SpawningItem : BoolEvent
    {
        /// <summary>
        /// Gets or sets the item that will be spawned.
        /// </summary>
        public BasePickup Pickup { get; set; }

        /// <summary>
        /// Gets the item's owner.
        /// </summary>
        public Player Owner { get => Pickup?.Owner ?? PlayersList.host; }

        public SpawningItem(BasePickup pickup, bool allow)
        {
            Pickup = pickup;
            IsAllowed = allow;
        }
    }
}
