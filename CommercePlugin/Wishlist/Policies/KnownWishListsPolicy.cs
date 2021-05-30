using Sitecore.Commerce.Core;

namespace Website.Plugin.WishLists.Policies
{
    public class KnownWishListsPolicy : Policy
    {
        public string CustomerWishLists { get; set; } = "WishLists-ByCustomer-{0}";
    }
}
