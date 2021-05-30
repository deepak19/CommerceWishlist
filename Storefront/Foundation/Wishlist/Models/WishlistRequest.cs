using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Foundation.WishList.Models
{
    public class WishlistRequest
    {
        public string CatalogName { get; set; }
        public string wishListName { get; } = Constants.WishlistName;
        public string wishListId { get; set; }
        public WishlistRequestLine[] Lines { get; set; }
        public bool RemoveFromWishList { get; set; }
    }

    public class WishlistRequestLine
    {
        public string ProductId { get; set; }
        public string VariantId { get; set; }
        public int Quantity { get; set; } = 1;
        public int SubscriptionMonths { get; set; } = 0;
        public bool IsSubscription { get { return (SubscriptionMonths > 0); } }
    }
}