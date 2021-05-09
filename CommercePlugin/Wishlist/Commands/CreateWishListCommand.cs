using Website.Plugin.WishLists.Entities;
using Website.Plugin.WishLists.Pipelines;
using Microsoft.Extensions.Logging;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Commerce.Plugin.Customers;
using System.Threading.Tasks;

namespace Website.Plugin.WishLists.Commands
{
    public class CreateWishListCommand : CommerceCommand
    {
        private readonly GetCustomerCommand _getCustomerCommand;
        private readonly IFindEntityPipeline _findEntityPipeline;
        private readonly ICreateWishListPipeline _createWishListPipeline;
        private readonly GetWishListCommand _getWishListCommand;
        private readonly FindEntitiesInListCommand _findEntitiesInListCommand;


        public CreateWishListCommand(GetCustomerCommand getCustomerCommand, IFindEntityPipeline findEntityPipeline, ICreateWishListPipeline createWishListPipeline, GetWishListCommand getWishListCommand, FindEntitiesInListCommand findEntitiesInListCommand)
            : base()
        {
            _getCustomerCommand = getCustomerCommand;
            _findEntityPipeline = findEntityPipeline;
            _createWishListPipeline = createWishListPipeline;
            _getWishListCommand = getWishListCommand;
            _findEntitiesInListCommand = findEntitiesInListCommand;
        }
        public virtual async Task<WishList> Process(CommerceContext commerceContext, string wishListName)
        {
            var customerEntity = await _getCustomerCommand.Process(commerceContext, commerceContext.CurrentCustomerId()).ConfigureAwait(continueOnCapturedContext: false);
            if (customerEntity != null)
            {
                var wishlist = await _getWishListCommand.Process(commerceContext, wishListName).ConfigureAwait(continueOnCapturedContext: false); ;
                if (wishlist != null)
                    return wishlist;
                else
                {
                    using (CommandActivity.Start(commerceContext, this))
                    {
                        return await _createWishListPipeline.Run(wishListName, commerceContext.PipelineContextOptions).ConfigureAwait(continueOnCapturedContext: false); ;
                    }
                }
            }
            else
                commerceContext.Logger.LogWarning($"Customer not Found: {commerceContext.CurrentCustomerId()}");
            return null;
        }
    }
}
