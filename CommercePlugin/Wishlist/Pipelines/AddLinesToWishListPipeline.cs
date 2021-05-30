using Website.Plugin.WishLists.Entities;
using Website.Plugin.WishLists.Pipelines.Arguments;
using Microsoft.Extensions.Logging;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Website.Plugin.WishLists.Pipelines
{
    public class AddLinesToWishListPipeline : CommercePipeline<WishListLinesArg, WishList>, IAddLinesToWishListPipeline
    {
        public AddLinesToWishListPipeline(IPipelineConfiguration<IAddLinesToWishListPipeline> configuration, ILoggerFactory loggerFactory) : base(configuration, loggerFactory)
        {
        }
    }
}
