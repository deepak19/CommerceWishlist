using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Website.Plugin.WishLists.Models
{
    public class WishListLineRequest
    {
        public string ItemId { get; set; }
        public uint Quantity { get; set; }
        public bool IsSubscription { get { return (SubscriptionMonths > 0); } }
        public int SubscriptionMonths { get; set; }
    }
}
