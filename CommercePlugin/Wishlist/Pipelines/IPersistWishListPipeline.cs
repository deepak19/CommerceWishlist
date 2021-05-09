using Website.Plugin.WishLists.Entities;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Website.Plugin.WishLists.Pipelines
{
    [PipelineDisplayName("PersistWishListPipeline")]
    public interface IPersistWishListPipeline : IPipeline<WishList, WishList, CommercePipelineExecutionContext>
    {
    }
}
