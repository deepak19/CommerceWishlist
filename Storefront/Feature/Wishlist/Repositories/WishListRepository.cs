using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Website.Feature.WishList.Models;
using Website.Foundation.WishList.Managers;
using Website.Foundation.WishList.Models;
using Newtonsoft.Json;
using Sitecore.Commerce.XA.Foundation.Common.Context;
using Sitecore.Commerce.XA.Foundation.Common.ExtensionMethods;
using Sitecore.Commerce.XA.Foundation.Common.Models;
using Sitecore.Commerce.XA.Foundation.Common.Models.JsonResults;
using Sitecore.Commerce.XA.Foundation.Common.Repositories;
using Sitecore.Commerce.XA.Foundation.Connect;
using Sitecore.Diagnostics;
using Sitecore.SecurityModel;
using Sitecore.XA.Foundation.Mvc;


namespace Website.Feature.WishList.Repositories
{
    public class WishListRepository : BaseCommerceModelRepository, IWishListRepository
    {
        public IModelProvider ModelProvider { get; protected set; }
        public IWishlistManager WishlistManager { get; protected set; }
        public IStorefrontContext _storefrontContext;
        public WishListRepository(IModelProvider modelProvider, IWishlistManager wishlistManager, IStorefrontContext storefrontContext)
        {
            Assert.ArgumentNotNull(modelProvider, "modelProvider");
            Assert.ArgumentNotNull(wishlistManager, "wishlistManager");
            ModelProvider = modelProvider;
            WishlistManager = wishlistManager;
            _storefrontContext = storefrontContext;
        }

        public virtual WishListRenderingModel GetWishListModel()
        {
            WishListRenderingModel model = ModelProvider.GetModel<WishListRenderingModel>();
            try
            {
                Init(model);
                //prepare model data here
            }
            catch (Exception ex)
            {
                Log.Error($"{ex.ToString()}", this);
            }
            return model;
        }

        public virtual BaseJsonResult AddToWishList(IVisitorContext VisitorContext, WishlistRequest request)
        {
            Assert.ArgumentNotNull(request, "request");
            Assert.ArgumentNotNull(VisitorContext, "visitorContext");
            Assert.ArgumentNotNullOrEmpty(request.CatalogName, "catalogName");
            Assert.ArgumentNotNull(request.Lines, "Lines");
            BaseJsonResult model = ModelProvider.GetModel<BaseJsonResult>();
            var managerResponse = WishlistManager.AddLinesToWishList(VisitorContext, request);
            if (!managerResponse.ServiceProviderResult.Success)
            {
                model.SetErrors(managerResponse.ServiceProviderResult);
                return model;
            }
            //AddWishlistToUser(request.Lines, request.CatalogName);
            model.Success = true;
            return model;
        }
        public virtual BaseJsonResult RemoveFromWishList(IVisitorContext VisitorContext, WishlistRequest request)
        {
            Assert.ArgumentNotNull(request, "request");
            Assert.ArgumentNotNull(VisitorContext, "visitorContext");
            Assert.ArgumentNotNullOrEmpty(request.CatalogName, "catalogName");
            Assert.ArgumentNotNull(request.Lines, "Lines");
            BaseJsonResult model = ModelProvider.GetModel<BaseJsonResult>();
            var managerResponse = WishlistManager.RemoveWishListLines(VisitorContext, request);
            if (!managerResponse.ServiceProviderResult.Success)
            {
                model.SetErrors(managerResponse.ServiceProviderResult);
                return model;
            }
            //RemoveWishlistFromUser(request.Lines, request.CatalogName);
            model.Success = true;
            return model;
        }

        public virtual WishListJsonResult GetWishListJsonResult(IVisitorContext VisitorContext, string wishListName = Website.Foundation.WishList.Models.Constants.WishlistName)
        {
            Assert.ArgumentNotNullOrEmpty(wishListName, "wishListName");
            WishListJsonResult model = ModelProvider.GetModel<WishListJsonResult>();
            var managerResponse = WishlistManager.GetWishList(VisitorContext, wishListName, _storefrontContext.CurrentStorefront.ShopName);
            if (managerResponse.ServiceProviderResult.Success)
            {
                model.Success = true;
                if (managerResponse.Result != null)
                {
                    //prepare model data here

                }
                return model;
            }
            model.SetErrors(managerResponse.ServiceProviderResult);
            return model;
        }


        #region Wishlist Saving on Sitecore User Object
        public virtual bool GetWishListonUser(IVisitorContext VisitorContext, string wishListName = Website.Foundation.WishList.Models.Constants.WishlistName)
        {
            Assert.ArgumentNotNullOrEmpty(wishListName, "wishListName");
            var managerResponse = WishlistManager.GetWishList(VisitorContext, wishListName, _storefrontContext.CurrentStorefront.ShopName);
            if (managerResponse.ServiceProviderResult.Success)
            {
                if (managerResponse.Result != null)
                {
                    //SaveWishlistOnUser(managerResponse.Result);// Enable it to store wishlist on user object.
                    return true;
                }
            }
            return false;
        }

        public void SaveWishlistOnUser(Sitecore.Commerce.Entities.WishLists.WishList wishlist)
        {
            //We are storing wishlist productids on user object, to avoid too many biztool call for each Product Detail page load
            if (Sitecore.Context.User.IsAuthenticated)
            {
                var ids = new List<string>();
                foreach (var item in wishlist.Lines)
                {
                    var product = item.Product as Sitecore.Commerce.Engine.Connect.Entities.CommerceCartProduct;
                    ids.Add(this.GetProductId(product.ProductCatalog, product.ProductId, product.ProductVariantId));
                }
                SetWishListCustomProperty(ids);
            }
        }

        public void RemoveWishlistFromUser(WishlistRequestLine[] lines, string catalogName)
        {
            if (!Sitecore.Context.User.IsAuthenticated) return;
            if (Sitecore.Context.User.Profile.GetCustomProperty(Website.Foundation.WishList.Models.Constants.WishListKeyForProfile) != null)
            {
                var ids = JsonConvert.DeserializeObject<List<string>>(Sitecore.Context.User.Profile.GetCustomProperty(Website.Foundation.WishList.Models.Constants.WishListKeyForProfile).ToString());
                if(ids != null && ids.Any())
                {
                    ids.Distinct();
                    foreach (var line in lines)
                    {
                        ids.Remove(this.GetProductId(catalogName, line.ProductId, line.VariantId));
                    }
                    SetWishListCustomProperty(ids);
                }
            }
        }
        public void AddWishlistToUser(WishlistRequestLine[] lines, string catalogName)
        {
            try
            {
                if (!Sitecore.Context.User.IsAuthenticated) return;
                var ids = new List<string>();
                if (Sitecore.Context.User.Profile.GetCustomProperty(Website.Foundation.WishList.Models.Constants.WishListKeyForProfile) != null)
                {
                    var wishlistIds = JsonConvert.DeserializeObject<List<string>>(Sitecore.Context.User.Profile.GetCustomProperty(Website.Foundation.WishList.Models.Constants.WishListKeyForProfile).ToString());
                    if (wishlistIds != null && wishlistIds.Any())
                        ids = wishlistIds;
                }
                ids.Distinct();
                foreach (var line in lines)
                {
                    ids.Add(this.GetProductId(catalogName, line.ProductId, line.VariantId));
                }
                SetWishListCustomProperty(ids);
            }
            catch (Exception ex)
            {
                Log.Error($"AddWishlistToUser: {ex.ToString()}", this); ;
            }
        }

        private static void SetWishListCustomProperty(List<string> ids)
        {
            using (new SecurityDisabler())
            {
                Sitecore.Context.User.Profile.SetCustomProperty(Website.Foundation.WishList.Models.Constants.WishListKeyForProfile, JsonConvert.SerializeObject(ids.Distinct()));
                Sitecore.Context.User.Profile.Save();
            }
        }

        #endregion

        private string GetProductId(string catalogName, string productId, string variantId)
        {
            return $"{catalogName}|{productId}|{variantId}";
        }

        
    }
}
