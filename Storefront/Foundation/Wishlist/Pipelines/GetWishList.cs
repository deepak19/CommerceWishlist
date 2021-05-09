using Sitecore.Commerce.Pipelines;
using Sitecore.Commerce.Services;
using Sitecore.Commerce.Services.WishLists;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Foundation.WishList.Pipelines
{
    public class GetWishList: WishListPipelineProcessor
    {
        public override void Process(ServicePipelineArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            Assert.ArgumentNotNull(args.Request, "args.Request");
            Assert.ArgumentNotNull(args.Result, "args.Result");
            Assert.ArgumentCondition(args.Request is GetWishListRequest, "args.Request", "args.Request must be GetWishListRequest");
            Assert.ArgumentCondition(args.Result is GetWishListResult, "args.Result", "args.Result must be GetWishListResult");
            GetWishListRequest request = (GetWishListRequest)args.Request;
            GetWishListResult result = (GetWishListResult)args.Result;
            try
            {
                var wishList = base.GetWishlist(request);
                if (wishList != null)
                {
                    result.Success = true;
                    result.WishList = base.TranslateWishListToEntity(result.WishList, wishList);
                }
                else
                    result.Success = false;
            }
            catch (Exception ex)
            {
                Log.Error($"{ex.ToString()}", this);
                result.Success = false;
                result.SystemMessages.Add(base.CreateSystemMessage(ex));
            }
            base.Process(args);
        }
    }
}