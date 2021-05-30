using Website.Plugin.WishLists.Entities;
using Website.Plugin.WishLists.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Website.Plugin.WishLists.Pipelines
{
    [PipelineDisplayName("AddLinesToWishListPipeline")]
    public interface IAddLinesToWishListPipeline : IPipeline<WishListLinesArg, WishList, CommercePipelineExecutionContext>
    {
    }
}
