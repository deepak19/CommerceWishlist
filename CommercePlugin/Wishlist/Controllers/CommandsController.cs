using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.OData;
using Website.Plugin.WishLists.Commands;
using Website.Plugin.WishLists.Components;
using Website.Plugin.WishLists.Models;
using Website.Plugin.WishLists.Pipelines.Arguments;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;

namespace Website.Plugin.WishLists.Controllers
{
    [Route("api")]
    public class CommandsController : CommerceController
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Sitecore.Commerce.Plugin.Sample.CommandsController" /> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="globalEnvironment">The global environment.</param>
        public CommandsController(IServiceProvider serviceProvider, CommerceEnvironment globalEnvironment)
            : base(serviceProvider, globalEnvironment)
        {
        }

        [HttpPost]
        [Route("CreateWishList")]
        public async Task<IActionResult> CreateWishList([FromBody] ODataActionParameters value)
        {
            if (!base.ModelState.IsValid || value == null)
            {
                return new BadRequestObjectResult(base.ModelState);
            }
            if (!value.ContainsKey("wishListName") || string.IsNullOrEmpty(value["wishListName"]?.ToString()))
            {
                return new BadRequestObjectResult(value);
            }
            var command = Command<CreateWishListCommand>();
            var result = await command.Process(CurrentContext, value["wishListName"].ToString()).ConfigureAwait(continueOnCapturedContext: false);
            if (result != null)
            {
                return new ObjectResult(command);
            }
            return null;
        }

        [HttpPost]
        [Route("DeleteWishList")]
        public async Task<IActionResult> DeleteWishList([FromBody] ODataActionParameters value)
        {
            if (!base.ModelState.IsValid || value == null)
            {
                return new BadRequestObjectResult(base.ModelState);
            }
            if (!value.ContainsKey("wishListName") || string.IsNullOrEmpty(value["wishListName"]?.ToString()))
            {
                return new BadRequestObjectResult(value);
            }
            var command = Command<DeleteWishListCommand>();
            var result = await command.Process(CurrentContext, value["wishListName"].ToString()).ConfigureAwait(continueOnCapturedContext: false);
            if (result != null)
            {
                return new ObjectResult(command);
            }
            return null;
        }

        [HttpPost]
        [Route("AddLinesToWishList")]
        public async Task<IActionResult> AddLinesToWishList([FromBody] ODataActionParameters value)
        {
            if (!base.ModelState.IsValid || value == null)
            {
                return new BadRequestObjectResult(base.ModelState);
            }
            if (!value.ContainsKey("wishListName") || !value.ContainsKey("Lines") || string.IsNullOrEmpty(value["wishListName"]?.ToString()) || string.IsNullOrEmpty(value["Lines"]?.ToString()))
            {
                return new BadRequestObjectResult(value);
            }
            JArray jArray = (JArray)value["Lines"];
            var lines = jArray?.ToObject<List<WishListLineRequest>>();
            var arg = new WishListLinesArg();

            foreach (var line in lines)
            {

                SellableItem sellableItem = await Command<GetSellableItemCommand>().Process(CurrentContext, line.ItemId, filterVariations: true).ConfigureAwait(continueOnCapturedContext: false);
                if (sellableItem == null)
                {
                    return null;
                }
                arg.Lines.Add(new WishListComponent
                {
                    ItemId = line.ItemId,
                    Quantity = line.Quantity,
                    IsSubscription = line.IsSubscription,
                    SubscriptionMonths = line.SubscriptionMonths
                });
            }

            var command = Command<AddLinesToWishListCommand>();
            var result = await command.Process(CurrentContext, value["wishListName"].ToString(), arg).ConfigureAwait(continueOnCapturedContext: false);
            if (result != null)
            {
                return new ObjectResult(command);
            }
            return null;
        }

        [HttpPost]
        [Route("RemoveWishListLines")]
        public async Task<IActionResult> RemoveWishListLines([FromBody] ODataActionParameters value)
        {
            if (!base.ModelState.IsValid || value == null)
            {
                return new BadRequestObjectResult(base.ModelState);
            }
            if (!value.ContainsKey("wishListName") || !value.ContainsKey("Lines") || string.IsNullOrEmpty(value["wishListName"]?.ToString()) || string.IsNullOrEmpty(value["Lines"]?.ToString()))
            {
                return new BadRequestObjectResult(value);
            }
            JArray jArray = (JArray)value["Lines"];
            var lines = jArray?.ToObject<List<WishListLineRequest>>();
            var arg = new WishListLinesArg();
            foreach (var line in lines)
            {
                arg.Lines.Add(new WishListComponent
                {
                    ItemId = line.ItemId,
                    Quantity = line.Quantity,
                    IsSubscription = line.IsSubscription,
                    SubscriptionMonths = line.SubscriptionMonths
                });
            }
            var command = Command<RemoveWishListLinesCommand>();
            var result = await command.Process(CurrentContext, value["wishListName"].ToString(), arg).ConfigureAwait(continueOnCapturedContext: false);
            if (result != null)
            {
                return new ObjectResult(command);
            }
            return null;
        }

        [HttpPost]
        [Route("GetWishList")]
        public async Task<IActionResult> GetWishList([FromBody] ODataActionParameters value)
        {
            if (!base.ModelState.IsValid || value == null)
            {
                return new BadRequestObjectResult(base.ModelState);
            }
            if (!value.ContainsKey("Id") || string.IsNullOrEmpty(value["Id"]?.ToString()))
            {
                return new BadRequestObjectResult(value);
            }
            var command = Command<GetWishListCommand>();
            var result = await command.Process(CurrentContext, value["Id"].ToString()).ConfigureAwait(continueOnCapturedContext: false);
            if (result != null)
            {
                return new ObjectResult(command);
            }
            return null;
        }

        [HttpPost]
        [Route("GetWishLists")]
        public async Task<IActionResult> GetWishLists()
        {
            if (!base.ModelState.IsValid)
            {
                return new BadRequestObjectResult(base.ModelState);
            }
            var command = Command<GetWishListsCommand>();
            var result = await command.Process(CurrentContext).ConfigureAwait(continueOnCapturedContext: false);
            if (result != null)
            {
                return new ObjectResult(command);
            }
            return null;
        }
    }
}

