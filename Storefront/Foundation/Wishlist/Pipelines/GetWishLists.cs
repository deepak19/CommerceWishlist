using Microsoft.OData.Client;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Engine.Connect;
using Sitecore.Commerce.Entities.WishLists;
using Sitecore.Commerce.Pipelines;
using Sitecore.Commerce.Plugin.ManagedLists;
using Sitecore.Commerce.ServiceProxy;
using Sitecore.Commerce.Services.WishLists;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Website.Foundation.WishList.Pipelines
{
    public class GetWishLists : WishListPipelineProcessor
    {
        public override void Process(ServicePipelineArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            Assert.ArgumentNotNull(args.Request, "args.Request");
            Assert.ArgumentNotNull(args.Result, "args.Result");
            Assert.ArgumentCondition(args.Request is GetWishListsRequest, "args.Request", "args.Request must be GetWishListsRequest");
            Assert.ArgumentCondition(args.Result is GetWishListsResult, "args.Result", "args.Result must be GetWishListsResult");
            GetWishListsRequest request = (GetWishListsRequest)args.Request;
            GetWishListsResult result = (GetWishListsResult)args.Result;
            try
            {
                var container = EngineConnectUtility.GetShopsContainer(shopName: request.Shop.Name, customerId: request.UserId);
                ManagedList value = Proxy.GetValue(container.GetList(string.Format(CultureInfo.InvariantCulture, "WishLists-ByCustomer-{0}", request.UserId), "Website.Plugin.WishLists.Entities.WishList, Website.Plugin.WishLists", 0, 100).Expand("Items($expand=Components)"));
                if (value.Items != null)
                {
                    var wishlistHeaders = new List<WishListHeader>();

                    result.Success = true;
                    if (value.Items.Any())
                    {
                        wishlistHeaders.AddRange(base.TranslateWishListHeaders(value.Items));
                    }
                    result.WishLists = new System.Collections.ObjectModel.ReadOnlyCollection<WishListHeader>(wishlistHeaders);
                }
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