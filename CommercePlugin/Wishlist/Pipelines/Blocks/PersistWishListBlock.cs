using Website.Plugin.WishLists.Entities;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Website.Plugin.WishLists.Pipelines.Blocks
{
    [PipelineDisplayName("WishList.block.PersistWishList")]
    public class PersistWishListBlock : PipelineBlock<WishList, WishList, CommercePipelineExecutionContext>
    {
        private readonly IPersistEntityPipeline _persistPipeline;
        public PersistWishListBlock(IPersistEntityPipeline persistEntityPipeline)
        : base((string)null)
        {
            _persistPipeline = persistEntityPipeline;
        }


        public override async Task<WishList> Run(WishList arg, CommercePipelineExecutionContext context)
        {
            if (arg == null)
            {
                return null;
            }
            PersistEntityArgument arg2 = new PersistEntityArgument(arg);
            var result = await _persistPipeline.Run(arg2, context).ConfigureAwait(continueOnCapturedContext: false);
            if (result.Success)
                return result.Entity as WishList;
            return null;
        }
    }
}
