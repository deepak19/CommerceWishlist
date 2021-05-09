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
    public class GetWishListsCommand : CommerceCommand
    {
        private readonly GetCustomerCommand _getCustomerCommand;
        private readonly FindEntitiesInListCommand _findEntitiesInListCommand;


        public GetWishListsCommand(GetCustomerCommand getCustomerCommand, FindEntitiesInListCommand findEntitiesInListCommand)
            : base()
        {
            _getCustomerCommand = getCustomerCommand;
            _findEntitiesInListCommand = findEntitiesInListCommand;
        }
        public virtual async Task<List<WishList>> Process(CommerceContext commerceContext)
        {
            var listName = string.Format(CultureInfo.InvariantCulture, commerceContext.GetPolicy<KnownWishListsPolicy>().CustomerWishLists, commerceContext.CurrentCustomerId());
            var commerceList = await _findEntitiesInListCommand.Process<WishList>(commerceContext, listName, 0, int.MaxValue).ConfigureAwait(continueOnCapturedContext: false);

            if (commerceList != null && commerceList.Items.Any())
            {
                return commerceList.Items;
            }
            return null;
        }
    }
}
