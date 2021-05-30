using Website.Plugin.WishLists.Entities;
using Website.Plugin.WishLists.Policies;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Commerce.Plugin.ManagedLists;
using System.Globalization;
using Sitecore.Framework.Conditions;

namespace Website.Plugin.WishLists.Pipelines.Blocks
{
    [PipelineDisplayName("WishLists.CreateWishListBlockBlock")]
    public class CreateWishListBlock : PipelineBlock<string, WishList, CommercePipelineExecutionContext>
    {
        private readonly IFindEntityPipeline _findEntityPipeline;
        public CreateWishListBlock(IFindEntityPipeline findEntityPipeline)
            : base((string)null)
        {
            _findEntityPipeline = findEntityPipeline;
        }

        public override Task<WishList> Run(string arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNullOrEmpty(base.Name + ": wishlistname can not be null or empty.");

            var wishList = new WishList(arg);
            string text = $"{CommerceEntity.IdPrefix<WishList>()}{Guid.NewGuid():N}";
            wishList.Id = text;
            wishList.FriendlyId = text;
            wishList.ShopName = context.CommerceContext.CurrentShopName();
            wishList.CustomerId = context.CommerceContext.CurrentCustomerId();


            var listMembershipsComponent = new ListMembershipsComponent();
            listMembershipsComponent.Memberships.Add(CommerceEntity.ListName<WishList>());
            listMembershipsComponent.Memberships.Add(string.Format(CultureInfo.InvariantCulture, context.GetPolicy<KnownWishListsPolicy>().CustomerWishLists, wishList.CustomerId));

            wishList.SetComponent(listMembershipsComponent);
            return Task.FromResult(wishList);
        }
    }
}
