using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Website.Plugin.WishLists.Entities;
using Website.Plugin.WishLists.Pipelines;
using Website.Plugin.WishLists.Pipelines.Arguments;
using Microsoft.Extensions.Logging;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Commerce.Plugin.Customers;
using Sitecore.Framework.Pipelines;

namespace Website.Plugin.WishLists.Commands
{
    public class RemoveWishListLinesCommand : CommerceCommand
    {
        private readonly GetCustomerCommand _getCustomerCommand;
        private readonly GetWishListCommand _getWishListCommand;
        private readonly IRemoveWishlistLinesPipeline _removeWishlistLinesPipeline;

        public RemoveWishListLinesCommand(GetCustomerCommand getCustomerCommand, GetWishListCommand getWishListCommand, RemoveWishlistLinesPipeline removeWishlistLinesPipeline)
            : base()
        {
            _getCustomerCommand = getCustomerCommand;
            _getWishListCommand = getWishListCommand;
            _removeWishlistLinesPipeline = removeWishlistLinesPipeline;
        }
        public virtual async Task<WishList> Process(CommerceContext commerceContext, string wishListName, WishListLinesArg arg)
        {
            WishList wishList;
            var customerEntity = await _getCustomerCommand.Process(commerceContext, commerceContext.CurrentCustomerId()).ConfigureAwait(continueOnCapturedContext: false);
            if (customerEntity != null)
            {
                wishList = await _getWishListCommand.Process(commerceContext, wishListName).ConfigureAwait(continueOnCapturedContext: false);
                if (wishList != null)
                {
                    arg.WishList = wishList;
                    return await _removeWishlistLinesPipeline.Run(arg, commerceContext.PipelineContextOptions).ConfigureAwait(continueOnCapturedContext: false); ;
                }
            }
            else
                commerceContext.Logger.LogWarning($"Customer not Found: {commerceContext.CurrentCustomerId()}");
            return null;
        }
    }
}
