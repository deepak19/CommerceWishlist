using Website.Plugin.WishLists.Components;
using Website.Plugin.WishLists.Entities;
using Website.Plugin.WishLists.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Website.Plugin.WishLists.Pipelines.Blocks
{
    [PipelineDisplayName("WishLists.RemoveWishListLinesBlock")]
    public class RemoveWishListLinesBlock : PipelineBlock<WishListLinesArg, WishList, CommercePipelineExecutionContext>
    {
        public RemoveWishListLinesBlock(IFindEntityPipeline findEntityPipeline)
            : base((string)null)
        {
        }
        public override async Task<WishList> Run(WishListLinesArg arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull(base.Name + ": AddWishListLines arg can not be null.");
            Condition.Requires(arg.WishList).IsNotNull(base.Name + ": AddWishListLines.WishList arg can not be null.");
            var wishList = arg.WishList;
            foreach (var reqLine in arg.Lines)
            {
                if (string.IsNullOrEmpty(reqLine.ItemId))
                {
                    CommercePipelineExecutionContext commercePipelineExecutionContext = context;
                    commercePipelineExecutionContext.Abort(await context.CommerceContext.AddMessage(context.GetPolicy<KnownResultCodes>().Error, "InvalidWishlist Remove Req", new object[1]
                    {arg.WishList.Name }, $"Can not remove empty line").ConfigureAwait(continueOnCapturedContext: false), context);
                    return null;
                }

                //Func<WishListComponent, bool> predicate = x => x.ItemId.Equals(reqLine.ItemId, StringComparison.OrdinalIgnoreCase) && (x.IsSubscription == reqLine.IsSubscription);
                Func<WishListComponent, bool> predicate = x => x.ItemId.Equals(reqLine.ItemId, StringComparison.OrdinalIgnoreCase);
                var exisitingLine = wishList.Lines.Where(predicate).FirstOrDefault();
                if (exisitingLine != null)
                    wishList.Lines.Remove(exisitingLine);
            }
            return wishList;
        }
    }
}
