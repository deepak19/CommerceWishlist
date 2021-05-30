using Website.Plugin.WishLists.Entities;
using Microsoft.Extensions.Logging;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Website.Plugin.WishLists.Pipelines
{
    public class PersistWishListPipeline : CommercePipeline<WishList, WishList>, IPersistWishListPipeline
    {
        public PersistWishListPipeline(IPipelineConfiguration<IPersistWishListPipeline> configuration, ILoggerFactory loggerFactory) : base(configuration, loggerFactory)
        {
        }
    }
}
