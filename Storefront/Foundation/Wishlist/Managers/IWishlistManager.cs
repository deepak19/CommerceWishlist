using Website.Foundation.WishList.Models;
using Sitecore.Commerce.Entities.WishLists;
using Sitecore.Commerce.Services.WishLists;
using Sitecore.Commerce.XA.Foundation.Connect;
using Sitecore.Commerce.XA.Foundation.Connect.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Foundation.WishList.Managers
{
    public interface IWishlistManager
    {
        ManagerResponse<AddLinesToWishListResult, Sitecore.Commerce.Entities.WishLists.WishList> AddLinesToWishList(IVisitorContext visitorContext, WishlistRequest wishlistRequest);
        ManagerResponse<RemoveWishListLinesResult, Sitecore.Commerce.Entities.WishLists.WishList> RemoveWishListLines(IVisitorContext visitorContext, WishlistRequest wishlistRequest);
        ManagerResponse<GetWishListsResult, IList<WishListHeader>> GetWishLists(IVisitorContext visitorContext, string shopName);
        ManagerResponse<GetWishListResult, Sitecore.Commerce.Entities.WishLists.WishList> GetWishList(IVisitorContext visitorContext , string wishListName, string shopName);

    }
}