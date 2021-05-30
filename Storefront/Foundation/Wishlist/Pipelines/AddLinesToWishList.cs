using Website.Plugin.WishLists.Models;
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
    public class AddLinesToWishList : WishListPipelineProcessor
    {
        public override void Process(ServicePipelineArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            Assert.ArgumentNotNull(args.Request, "args.Request");
            Assert.ArgumentNotNull(args.Result, "args.Result");
            Assert.ArgumentCondition(args.Request is AddLinesToWishListRequest, "args.Request", "args.Request must be AddLinesToWishListRequest");
            Assert.ArgumentCondition(args.Result is AddLinesToWishListResult, "args.Result", "args.Result must be AddLinesToWishListResult");
            AddLinesToWishListRequest request = (AddLinesToWishListRequest)args.Request;
            AddLinesToWishListResult result = (AddLinesToWishListResult)args.Result;
            try
            {
                var lines = new List<WishListLineRequest>();
                foreach (var item in request.Lines)
                {
                    var commerceProduct = item.Product as Sitecore.Commerce.Engine.Connect.Entities.CommerceCartProduct;
                    var lineRequest = new WishListLineRequest();
                    lineRequest.ItemId = $"{commerceProduct.ProductCatalog}|{commerceProduct.ProductId}|{commerceProduct.ProductVariantId}";
                    lineRequest.Quantity = Decimal.ToInt32(item.Quantity);
                    bool isSubscription;
                    if (item.Product.ContainsKey(Models.Constants.IsSubscription) && Boolean.TryParse(item.Product.GetPropertyValue(Models.Constants.IsSubscription).ToString(), out isSubscription) && isSubscription)
                    {
                        if (item.Product.ContainsKey(Models.Constants.SubscriptionMonths))
                            lineRequest.SubscriptionMonths = int.Parse(item.Product.GetPropertyValue(Models.Constants.SubscriptionMonths).ToString());
                    }
                    lines.Add(lineRequest);
                }
                var container = EngineConnectUtility.GetShopsContainer(shopName: request.Shop.Name, customerId: request.WishList.CustomerId);
                var value = Proxy.GetValue(container.AddLinesToWishList(request.WishList.Name, lines));
                if (value.IsCompleted)
                {

                    result.Success = value.IsCompleted;
                    result.WishList = request.WishList;
                    result.AddedLines = new System.Collections.ObjectModel.ReadOnlyCollection<WishListLine>(request.Lines.ToList());
                }
                else
                {
                    Log.Info($"AddLinesToWishList: fail", this);
                    if (value.Messages.Any()) Log.Error($"{value.Messages[0].Text}", this);
                    result.WishList = new Sitecore.Commerce.Entities.WishLists.WishList();
                    result.AddedLines = new System.Collections.ObjectModel.ReadOnlyCollection<WishListLine>(Array.Empty<WishListLine>());
                }
            }
            catch (ArgumentException ex)
            {
                Log.Error($"{ex.ToString()}", this);
                result.Success = false;
                result.SystemMessages.Add(base.CreateSystemMessage(ex));
            }
            catch (DataServiceQueryException ex2)
            {
                Log.Error($"{ex2.ToString()}", this);
                result.Success = false;
                result.SystemMessages.Add(base.CreateSystemMessage(ex2));
            }
            base.Process(args);
        }
    }
}