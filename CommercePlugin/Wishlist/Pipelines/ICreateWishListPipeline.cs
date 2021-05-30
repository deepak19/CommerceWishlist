using Website.Plugin.WishLists.Entities;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Website.Plugin.WishLists.Pipelines
{
    [PipelineDisplayName("CreateWishListPipeline")]
    public interface ICreateWishListPipeline : IPipeline<string, WishList, CommercePipelineExecutionContext>
    {
    }
}
