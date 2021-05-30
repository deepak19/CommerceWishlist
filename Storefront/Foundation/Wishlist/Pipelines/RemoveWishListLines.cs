using Website.Plugin.WishLists.Models;
using Sitecore.Commerce.Engine.Connect;
using Sitecore.Commerce.Entities.WishLists;
using Sitecore.Commerce.Pipelines;
using Sitecore.Commerce.ServiceProxy;
using Sitecore.Commerce.Services.WishLists;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Foundation.WishList.Pipelines
{
    public class RemoveWishListLines : WishListPipelineProcessor
    {
        public override void Process(ServicePipelineArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            Assert.ArgumentNotNull(args.Request, "args.Request");
            Assert.ArgumentNotNull(args.Result, "args.Result");
            Assert.ArgumentCondition(args.Request is RemoveWishListLinesRequest, "args.Request", "args.Request must be RemoveWishListLinesRequest");
            Assert.ArgumentCondition(args.Result is RemoveWishListLinesResult, "args.Result", "args.Result must be RemoveWishListLinesResult");
            RemoveWishListLinesRequest request = (RemoveWishListLinesRequest)args.Request;
            RemoveWishListLinesResult result = (RemoveWishListLinesResult)args.Result;
            try
            {
                var lines = new List<WishListLineRequest>();
                foreach (var item in request.LineIds)
                {
                    var lineRequest = new WishListLineRequest();
                    lineRequest.ItemId = item;
                    if (item.Contains(Models.Constants.SubscriptionKey))
                    {
                        lineRequest.ItemId = item.Replace(Models.Constants.SubscriptionKey, "");
                        lineRequest.SubscriptionMonths = 1;//Dummy value to intentify that subscription line need to be removed
                    }
                    lines.Add(lineRequest);
                    Log.Info($"RemoveWishListLines: {lineRequest.ItemId}", this);
                }
                var container = EngineConnectUtility.GetShopsContainer(shopName: request.Shop.Name, customerId: request.WishList.CustomerId);
                var value = Proxy.GetValue(container.RemoveWishListLines(request.WishList.Name, lines));
                if (value.IsCompleted)
                {
                    result.Success = value.IsCompleted;
                    result.WishList = request.WishList;
                    result.RemovedLines = new System.Collections.ObjectModel.ReadOnlyCollection<WishListLine>(new List<WishListLine>());
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