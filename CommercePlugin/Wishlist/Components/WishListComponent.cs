using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;

namespace Website.Plugin.WishLists.Components
{
    public class WishListComponent : Component
    {
        public string ItemId { get; set; }
        public uint Quantity { get; set; }
        public bool IsSubscription { get; set; }
        public int SubscriptionMonths { get; set; }

        public WishListComponent()
        {
            Id = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            IsSubscription = false;
            SubscriptionMonths = 0;
        }
    }
}
