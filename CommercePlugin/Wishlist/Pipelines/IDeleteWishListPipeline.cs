using Website.Plugin.WishLists.Entities;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Website.Plugin.WishLists.Pipelines
{
    [PipelineDisplayName("DeleteWishListPipeline")]
    public interface IDeleteWishListPipeline : IPipeline<WishList, WishList, CommercePipelineExecutionContext>
    {
    }
}
