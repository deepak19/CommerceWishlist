using Website.Feature.WishList.Models;
using Website.Foundation.WishList.Models;
using Sitecore.Commerce.XA.Foundation.Common.Models.JsonResults;
using Sitecore.Commerce.XA.Foundation.Connect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Feature.WishList.Repositories
{
    public interface IWishListRepository
    {
        BaseJsonResult AddToWishList(IVisitorContext VisitorContext, WishlistRequest request);
        BaseJsonResult RemoveFromWishList(IVisitorContext VisitorContext, WishlistRequest request);
        WishListJsonResult GetWishListJsonResult(IVisitorContext VisitorContext, string wishListName = Constants.WishlistName);
    }
}