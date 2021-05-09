using Sitecore.Commerce.Services.WishLists;
using Sitecore.Commerce.XA.Foundation.Connect.Managers;
using System.Collections.ObjectModel;
using Sitecore.Commerce.Entities.WishLists;
using Sitecore.Commerce.XA.Foundation.Common.Context;
using Sitecore.Commerce.XA.Foundation.Connect.Providers;
using Sitecore.Diagnostics;
using Sitecore.Configuration;
using Sitecore.Commerce.XA.Foundation.Common.Utils;
using System.Collections.Generic;
using Sitecore.Commerce.XA.Foundation.Connect;
using Website.Foundation.WishList.Models;
using Sitecore.Commerce.Engine.Connect.Entities;
using Sitecore.Commerce.Entities;

namespace Website.Foundation.WishList.Managers
{
  
    public class WishlistManager : IWishlistManager
    {
        public IStorefrontContext StorefrontContext
        {
            get;
            set;
        }
        public WishListServiceProvider WishListServiceProvider
        {
            get;
            set;
        }
        private IEntityFactory _entityFactory = Factory.CreateObject("entityFactory", assert: true) as IEntityFactory;
        public WishlistManager(IConnectServiceProvider connectServiceProvider, IStorefrontContext storefrontContext)
        {
            Assert.ArgumentNotNull(connectServiceProvider, "connectServiceProvider");
            Assert.ArgumentNotNull(storefrontContext, "storefrontContext");
            WishListServiceProvider = (WishListServiceProvider)Factory.CreateObject("wishListServiceProvider", assert: true);
            StorefrontContext = storefrontContext;
        }

        public virtual ManagerResponse<AddLinesToWishListResult, Sitecore.Commerce.Entities.WishLists.WishList> AddLinesToWishList(IVisitorContext visitorContext, WishlistRequest wishlistRequest)
        {
            Assert.ArgumentNotNull(visitorContext, "visitorContext");
            var wishlist = new Sitecore.Commerce.Entities.WishLists.WishList();
            wishlist.Name = wishlistRequest.wishListName;
            wishlist.ExternalId = wishlistRequest.wishListId;
            wishlist.CustomerId = visitorContext.CustomerId;
            var lines = new List<WishListLine>();
            foreach (var item in wishlistRequest.Lines)
            {
                var line = new WishListLine();
                CommerceCartProduct commerceCartProduct = _entityFactory.Create<CommerceCartProduct>("CartProduct");
                commerceCartProduct.ProductCatalog = wishlistRequest.CatalogName;
                commerceCartProduct.ProductId = item.ProductId;
                commerceCartProduct.ProductVariantId = item.VariantId;
                commerceCartProduct.SetPropertyValue(Models.Constants.IsSubscription, item.IsSubscription);
                commerceCartProduct.SetPropertyValue(Models.Constants.SubscriptionMonths, item.SubscriptionMonths);
                
                line.Product = commerceCartProduct;
                line.Quantity = item.Quantity;
                lines.Add(line);
            }
            AddLinesToWishListRequest request = new AddLinesToWishListRequest(wishlist, lines);
            var wishListResult = WishListServiceProvider.AddLinesToWishList(request);
            Helpers.LogSystemMessages(wishListResult.SystemMessages, wishListResult);
            return new ManagerResponse<AddLinesToWishListResult, Sitecore.Commerce.Entities.WishLists.WishList>(wishListResult, wishListResult.WishList);
        }

        public virtual ManagerResponse<RemoveWishListLinesResult, Sitecore.Commerce.Entities.WishLists.WishList> RemoveWishListLines(IVisitorContext visitorContext, WishlistRequest wishlistRequest)
        {
            Assert.ArgumentNotNull(visitorContext, "visitorContext");
            Assert.ArgumentNotNullOrEmpty(wishlistRequest.CatalogName, "CatalogName");
            var wishlist = new Sitecore.Commerce.Entities.WishLists.WishList();
            wishlist.Name = wishlistRequest.wishListName;
            wishlist.ExternalId = wishlistRequest.wishListId;
            wishlist.CustomerId = visitorContext.CustomerId;

            var line = new List<string>();
            foreach (var item in wishlistRequest.Lines)
            {
                if (string.IsNullOrEmpty(item.ProductId) || string.IsNullOrEmpty(item.VariantId)) continue;
                var productId = $"{wishlistRequest.CatalogName}|{item.ProductId}|{item.VariantId}";
                if (item.IsSubscription)
                {
                    productId = productId + Models.Constants.SubscriptionKey;//Adding a key to identify subscription item removal.
                }
                line.Add($"{wishlistRequest.CatalogName}|{item.ProductId}|{item.VariantId}");
            }

            RemoveWishListLinesRequest request = new RemoveWishListLinesRequest(wishlist, line);
            var wishListResult = WishListServiceProvider.RemoveWishListLines(request);
            Helpers.LogSystemMessages(wishListResult.SystemMessages, wishListResult);
            return new ManagerResponse<RemoveWishListLinesResult, Sitecore.Commerce.Entities.WishLists.WishList>(wishListResult, wishListResult.WishList);
        }
        public virtual ManagerResponse<GetWishListsResult, IList<WishListHeader>> GetWishLists(IVisitorContext visitorContext, string shopName)
        {
            Assert.ArgumentNotNull(visitorContext, "visitorContext");
            GetWishListsRequest request = new GetWishListsRequest(visitorContext.CustomerId, shopName);
            var wishListResult = WishListServiceProvider.GetWishLists(request);
            Helpers.LogSystemMessages(wishListResult.SystemMessages, wishListResult);
            return new ManagerResponse<GetWishListsResult, IList<WishListHeader>>(wishListResult, wishListResult.WishLists);
        }
        public virtual ManagerResponse<GetWishListResult, Sitecore.Commerce.Entities.WishLists.WishList> GetWishList(IVisitorContext visitorContext, string wishListName, string shopName)
        {
            Assert.ArgumentNotNull(visitorContext, "visitorContext");
            GetWishListRequest request = new GetWishListRequest(visitorContext.CustomerId, wishListName, shopName);
            var wishListResult = WishListServiceProvider.GetWishList(request);
            Helpers.LogSystemMessages(wishListResult.SystemMessages, wishListResult);
            return new ManagerResponse<GetWishListResult, Sitecore.Commerce.Entities.WishLists.WishList>(wishListResult, wishListResult.WishList);
        }
    }
}