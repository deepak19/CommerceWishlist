using Website.Plugin.WishLists.Commands;
using Website.Plugin.WishLists.Entities;
using Microsoft.AspNetCore.Mvc;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.OData;

namespace Website.Plugin.WishLists.Controllers
{
    [Microsoft.AspNetCore.OData.EnableQuery]
    [Route("api/WishLists")]
    public class WishListsController : CommerceController
    {
        public WishListsController(IServiceProvider serviceProvider, CommerceEnvironment globalEnvironment)
            : base(serviceProvider, globalEnvironment)
        {
        }

        [HttpGet]
        [Route("WishLists")]
        public async Task<IEnumerable<WishList>> Get()
        {
            return (await Command<FindEntitiesInListCommand>().Process<WishList>(CurrentContext, CommerceEntity.ListName<WishList>(), 0, int.MaxValue).ConfigureAwait(continueOnCapturedContext: false)).Items.ToList();
        }

        [HttpGet]
        [Route("WishLists({id})")]
        public async Task<IActionResult> Get([FromODataUri] string id)
        {
            if (!base.ModelState.IsValid || string.IsNullOrEmpty(id))
            {
                return ((ControllerBase)(object)this).NotFound();
            }
            WishList wishlist;
            if (id.Contains(CommerceEntity.IdPrefix<WishList>()))
                wishlist = await Command<FindEntityCommand>().Process<WishList>(CurrentContext,id).ConfigureAwait(continueOnCapturedContext: false);
            else
                wishlist = await Command<GetWishListCommand>().Process(CurrentContext, id).ConfigureAwait(continueOnCapturedContext: false);


            if (wishlist == null)
            {
                return ((ControllerBase)(object)this).NotFound();
            }
            return new ObjectResult(wishlist);
        }
    }
}