using Website.Feature.WishList.Models;
using Website.Feature.WishList.Repositories;
using Website.Foundation.WishList.Models;
using Sitecore.Commerce.XA.Foundation.Common.Context;
using Sitecore.Commerce.XA.Foundation.Common.Controllers;
using Sitecore.Commerce.XA.Foundation.Common.Models;
using Sitecore.Commerce.XA.Foundation.Common.Models.JsonResults;
using Sitecore.Commerce.XA.Foundation.Connect;
using System;
using System.Web.Mvc;
using System.Web.UI;

namespace Website.Feature.WishList.Controllers
{
    public class WishListController : BaseCommerceStandardController
    {
        private readonly IWishListRepository _wishListRepository;
        public IVisitorContext VisitorContext;
        public IModelProvider ModelProvider;
        public WishListController(IStorefrontContext storefrontContext, IContext sitecoreContext, IModelProvider modelProvider, IWishListRepository wishListRepository)
            : base(storefrontContext, sitecoreContext)
        {
            _wishListRepository = wishListRepository;
            ModelProvider = modelProvider;
            VisitorContext = Sitecore.Commerce.XA.Foundation.Connect.VisitorContext.Current;
        }

        [HttpPost]
        [Authorize]
        public ActionResult WishlistbyUser()
        {

            WishListJsonResult result;
            try
            {
                result = _wishListRepository.GetWishListJsonResult(VisitorContext);
            }
            catch (Exception ex)
            {
                result = ModelProvider.GetModel<WishListJsonResult>();
                result.SetErrors("WishlistbyUser", ex);
            }
            return Json(result);
        }



        [HttpPost]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public JsonResult RemoveorAddToWishList(WishlistRequest request)
        {
            BaseJsonResult baseJsonResult;
            try
            {
                if (!Sitecore.Context.User.IsAuthenticated)
                {
                    baseJsonResult = ModelProvider.GetModel<BaseJsonResult>();
                    baseJsonResult.Success = false;
                    baseJsonResult.Url = RedirectToLogin();
                }
                else
                {
                    if (request.RemoveFromWishList)
                        baseJsonResult = _wishListRepository.RemoveFromWishList(VisitorContext, request);
                    else
                        baseJsonResult = _wishListRepository.AddToWishList(VisitorContext, request);
                }
            }
            catch (Exception ex)
            {
                baseJsonResult = ModelProvider.GetModel<BaseJsonResult>();
                baseJsonResult.SetErrors("RemoveorAddToWishList", ex);
                baseJsonResult.SetError(ex.ToString());

            }
            return Json(baseJsonResult);
        }

        private string RedirectToLogin()
        {
            Sitecore.Sites.SiteContext site = Sitecore.Context.Site;
            return $"{site.LoginPage}?returnUrl={System.Web.HttpUtility.UrlEncode(base.Request.UrlReferrer.AbsolutePath)}";
        }
    }
}