using Sitecore.Commerce.Pipelines;
using Sitecore.Commerce.Services.WishLists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Commerce.Engine.Connect;
using Website.Plugin.WishLists;
using Sitecore.Commerce.ServiceProxy;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Commerce.Engine;
using Sitecore.Commerce.Engine.Connect.Entities;
using Sitecore.Commerce.Engine.Connect.Pipelines;
using Sitecore.Commerce.Entities.Carts;
using Sitecore.Diagnostics;
using Website.Plugin.WishLists.Entities;
using Sitecore.Commerce.Plugin.ManagedLists;
using System.Globalization;
using Sitecore.Commerce.Services;
using Sitecore.Commerce.Entities.WishLists;
using Sitecore.Commerce.Entities;
using Sitecore.Configuration;
using Sitecore.Commerce.Core;

namespace Website.Foundation.WishList.Pipelines
{
    public abstract class WishListPipelineProcessor : PipelineProcessor
    {
        private IEntityFactory _entityFactory = Factory.CreateObject("entityFactory", assert: true) as IEntityFactory;
        protected virtual Website.Plugin.WishLists.Entities.WishList GetWishlist(GetWishListRequest getWishListRequest)
        {
            Sitecore.Commerce.Engine.Container container = EngineConnectUtility.GetShopsContainer(shopName: getWishListRequest.Shop.Name, customerId: getWishListRequest.UserId);
            return Proxy.GetValue(container.WishLists.ByKey(getWishListRequest.WishListId).Expand("Lines($expand=ChildComponents),Components"));
        }

        internal SystemMessage CreateSystemMessage(Exception ex)
        {
            return new SystemMessage
            {
                Message = ex.ToString()
            };
        }

        internal Sitecore.Commerce.Entities.WishLists.WishList TranslateWishListToEntity(Sitecore.Commerce.Entities.WishLists.WishList destination, Website.Plugin.WishLists.Entities.WishList source)
        {
            var lines = new List<WishListLine>();
            destination = new Sitecore.Commerce.Entities.WishLists.WishList();
            destination.ExternalId = source.Id;
            destination.Name = source.Name;
            destination.ShopName = source.ShopName;
            destination.CustomerId = source.CustomerId;
            destination.IsFavorite = source.IsFavorite;
            foreach (var line in source.Lines)
            {
                string[] array = line.ItemId.Split("|".ToCharArray());

                CommerceCartProduct commerceCartProduct = _entityFactory.Create<CommerceCartProduct>("CartProduct");
                commerceCartProduct.ProductCatalog = array[0];
                commerceCartProduct.ProductId = array[1];
                commerceCartProduct.ProductVariantId = array[2];
                commerceCartProduct.SetPropertyValue(Models.Constants.IsSubscription, line.IsSubscription);
                commerceCartProduct.SetPropertyValue(Models.Constants.SubscriptionMonths, line.SubscriptionMonths);

                var enitityLine = new WishListLine();
                enitityLine.Product = commerceCartProduct;
                enitityLine.Quantity = line.Quantity;
                lines.Add(enitityLine);
            }
            destination.Lines = lines;
            return destination;
        }

        internal IEnumerable<WishListHeader> TranslateWishListHeaders(IEnumerable<CommerceEntity> items)
        {
            Assert.ArgumentNotNull(items, "wishlists");
            var list = new List<WishListHeader>();
            CommerceEntity[] source = (items as CommerceEntity[]) ?? items.ToArray();
            if (source.Any())
            {
                foreach (Website.Plugin.WishLists.Entities.WishList item in source.Cast<Website.Plugin.WishLists.Entities.WishList>())
                {
                    WishListHeader wishListHeader = _entityFactory.Create<WishListHeader>("WishListHeader");
                    wishListHeader.ExternalId = item.Id;
                    wishListHeader.ShopName = item.ShopName;
                    wishListHeader.Name = item.Name;
                    wishListHeader.CustomerId = item.CustomerId;
                    wishListHeader.IsFavorite = item.IsFavorite;
                    list.Add(wishListHeader);
                }
            }
            return list;
        }
    }
}