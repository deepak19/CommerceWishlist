using Website.Plugin.WishLists.Entities;
using Website.Plugin.WishLists.Pipelines;
using Website.Plugin.WishLists.Pipelines.Arguments;
using Microsoft.Extensions.Logging;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Commerce.Plugin.Customers;
using System.Threading.Tasks;

namespace Website.Plugin.WishLists.Commands
{
    public class AddLinesToWishListCommand : CommerceCommand
    {
        private readonly GetCustomerCommand _getCustomerCommand;
        private readonly GetWishListCommand _getWishListCommand;
        private readonly CreateWishListCommand _createWishListCommand;
        private readonly IAddLinesToWishListPipeline _addLinesToWishListPipeline;

        public AddLinesToWishListCommand(GetCustomerCommand getCustomerCommand, GetWishListCommand getWishListCommand, CreateWishListCommand createWishListCommand, IAddLinesToWishListPipeline addLinesToWishListPipeline)
            : base()
        {
            _getCustomerCommand = getCustomerCommand;
            _getWishListCommand = getWishListCommand;
            _createWishListCommand = createWishListCommand;
            _addLinesToWishListPipeline = addLinesToWishListPipeline;
        }
        public virtual async Task<WishList> Process(CommerceContext commerceContext, string wishListName, WishListLinesArg arg)
        {
            WishList wishList;
            var customerEntity = await _getCustomerCommand.Process(commerceContext, commerceContext.CurrentCustomerId()).ConfigureAwait(continueOnCapturedContext: false);
            if(customerEntity != null)
            {
                wishList = await _getWishListCommand.Process(commerceContext, wishListName).ConfigureAwait(continueOnCapturedContext: false);
                if (wishList == null)
                {
                    wishList = await _createWishListCommand.Process(commerceContext, wishListName).ConfigureAwait(continueOnCapturedContext: false);
                    commerceContext.Logger.LogWarning($"Wishlist created for: {commerceContext.CurrentCustomerId()}");
                }
                if (wishList != null)
                {
                    arg.WishList = wishList;
                    return await _addLinesToWishListPipeline.Run(arg, commerceContext.PipelineContextOptions).ConfigureAwait(continueOnCapturedContext: false); ;
                }
            }
            else
                commerceContext.Logger.LogWarning($"Customer not Found: {commerceContext.CurrentCustomerId()}");

            return null;
        }
    }
}
