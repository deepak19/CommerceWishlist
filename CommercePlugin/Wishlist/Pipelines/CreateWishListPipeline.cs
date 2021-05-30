using Website.Plugin.WishLists.Entities;
using Microsoft.Extensions.Logging;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Website.Plugin.WishLists.Pipelines
{
    public class CreateWishListPipeline : CommercePipeline<string, WishList>, ICreateWishListPipeline
    {
        public CreateWishListPipeline(IPipelineConfiguration<ICreateWishListPipeline> configuration, ILoggerFactory loggerFactory) : base(configuration, loggerFactory)
        {
        }
    }
}
