using Website.Plugin.WishLists.Entities;
using Website.Plugin.WishLists.Pipelines;
using Microsoft.Extensions.Logging;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Commerce.Plugin.Customers;
using System.Threading.Tasks;

namespace Website.Plugin.WishLists.Commands
{
    public class DeleteWishListCommand : CommerceCommand
    {
        private readonly GetCustomerCommand _getCustomerCommand;
        private readonly IDeleteWishListPipeline _deleteWishListPipeline;
        private readonly GetWishListCommand _getWishListCommand;

        public DeleteWishListCommand(GetCustomerCommand getCustomerCommand, IDeleteWishListPipeline deleteWishListPipeline, GetWishListCommand getWishListCommand)
            : base()
        {
            _getCustomerCommand = getCustomerCommand;
            _deleteWishListPipeline = deleteWishListPipeline;
            _getWishListCommand = getWishListCommand;
        }
        public virtual async Task<WishList> Process(CommerceContext commerceContext, string wishListName)
        {
            var customerEntity = await _getCustomerCommand.Process(commerceContext, commerceContext.CurrentCustomerId()).ConfigureAwait(continueOnCapturedContext: false);
            if (customerEntity != null)
            {
                var wishlist = await _getWishListCommand.Process(commerceContext, wishListName).ConfigureAwait(continueOnCapturedContext: false); ;
                if (wishlist != null)
                {
                    using (CommandActivity.Start(commerceContext, this))
                    {
                        return await _deleteWishListPipeline.Run(wishlist, commerceContext.PipelineContextOptions).ConfigureAwait(continueOnCapturedContext: false); ;
                    }
                }
            }
            else
                commerceContext.Logger.LogWarning($"Customer not Found: {commerceContext.CurrentCustomerId()}");
            return null;
        }
    }
}
