using Website.Plugin.WishLists.Entities;
using Website.Plugin.WishLists.Policies;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Commerce.Plugin.ManagedLists;
using System.Globalization;
using Sitecore.Framework.Conditions;

namespace Website.Plugin.WishLists.Pipelines.Blocks
{
    [PipelineDisplayName("WishLists.DeleteWishListBlock")]
    public class DeleteWishListBlock : PipelineBlock<WishList, WishList, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commander;
        public DeleteWishListBlock(CommerceCommander commander)
            : base((string)null)
        {
            _commander = commander;
        }

        public override async Task<WishList> Run(WishList arg, CommercePipelineExecutionContext context)
        {
            var deleteArg = new DeleteEntityArgument(arg);
            Condition.Requires(deleteArg.EntityToDelete).IsNotNull(base.Name + ": The wishlist item cannot be null");
            if (!(deleteArg.EntityToDelete is WishList))
            {
                return null;
            }
            var result = (await _commander.Pipeline<IDeleteEntityPipeline>().Run(deleteArg, context).ConfigureAwait(continueOnCapturedContext: false)).Success;
            if (result)
                return arg;
            else
                return null;
        }
    }
}
