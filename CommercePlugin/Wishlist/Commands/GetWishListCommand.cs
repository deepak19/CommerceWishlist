using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Website.Plugin.WishLists.Entities;
using Website.Plugin.WishLists.Policies;
using Microsoft.Extensions.Logging;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Commerce.Plugin.Customers;
using Sitecore.Framework.Pipelines;

namespace Website.Plugin.WishLists.Commands
{
    public class GetWishListCommand : CommerceCommand
    {
        private readonly IFindEntityPipeline _findEntityPipeline;
        private readonly FindEntitiesInListCommand _findEntitiesInListCommand;
        private Func<WishList, bool> predicate;

        public GetWishListCommand(IFindEntityPipeline findEntityPipeline, FindEntitiesInListCommand findEntitiesInListCommand)
            : base()
        {
            _findEntityPipeline = findEntityPipeline;
            _findEntitiesInListCommand = findEntitiesInListCommand;
        }
        public virtual async Task<WishList> Process(CommerceContext commerceContext, string id)
        {

            if (id.Contains(CommerceEntity.IdPrefix<WishList>()))//If request format is EntityID
                return await _findEntityPipeline.Run(new FindEntityArgument(typeof(WishList), id), commerceContext.PipelineContextOptions).ConfigureAwait(continueOnCapturedContext: false) as WishList;
            else
            {
                var listName = string.Format(CultureInfo.InvariantCulture, commerceContext.GetPolicy<KnownWishListsPolicy>().CustomerWishLists, commerceContext.CurrentCustomerId());
                CommerceList<WishList> commerceList = await _findEntitiesInListCommand.Process<WishList>(commerceContext, listName, 0, int.MaxValue).ConfigureAwait(continueOnCapturedContext: false);
                if (commerceList.Items != null && commerceList.Items.Any())
                {
                    predicate = x => x.Name.Equals(id, StringComparison.OrdinalIgnoreCase);//If request format is wishlistname
                    if (commerceList.Items.Any(predicate))
                    {
                        return commerceList.Items.First(predicate);
                    }
                }
            }

            return null;
        }
    }
}
