using Website.Plugin.WishLists.Entities;
using Website.Plugin.WishLists.Pipelines.Arguments;
using Microsoft.Extensions.Logging;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Website.Plugin.WishLists.Pipelines
{
    public class RemoveWishlistLinesPipeline : CommercePipeline<WishListLinesArg, WishList>, IRemoveWishlistLinesPipeline
    {
        public RemoveWishlistLinesPipeline(IPipelineConfiguration<IRemoveWishlistLinesPipeline> configuration, ILoggerFactory loggerFactory) : base(configuration, loggerFactory)
        {
        }
    }
}
