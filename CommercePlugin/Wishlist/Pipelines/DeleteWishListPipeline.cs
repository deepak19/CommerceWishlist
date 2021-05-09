using Website.Plugin.WishLists.Entities;
using Microsoft.Extensions.Logging;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Website.Plugin.WishLists.Pipelines
{
    public class DeleteWishListPipeline : CommercePipeline<WishList, WishList>, IDeleteWishListPipeline
    {
        public DeleteWishListPipeline(IPipelineConfiguration<IDeleteWishListPipeline> configuration, ILoggerFactory loggerFactory) : base(configuration, loggerFactory)
        {
        }
    }
}
