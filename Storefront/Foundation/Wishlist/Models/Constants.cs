using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Foundation.WishList.Models
{
    public static class Constants
    {
        public const string IsSubscription = "IsSubscription";
        public const string SubscriptionMonths = "SubscriptionMonths";
        public const string SubscriptionKey = "|" + nameof(SubscriptionKey);

        public const string WishlistName = "Default";//The current scope is not multi-wishlist. Hence, we will save all wished products into wishlist named 'Default'.  

        public const string WishListKeyForProfile = "WishList";
    }
}