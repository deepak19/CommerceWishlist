using Sitecore.Commerce.Entities.WishLists;
using Sitecore.Commerce.XA.Foundation.Common.Context;
using Sitecore.Commerce.XA.Foundation.Common.ExtensionMethods;
using Sitecore.Commerce.XA.Foundation.Common.Models.JsonResults;
using Sitecore.Commerce.XA.Foundation.Connect.Managers;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Website.Feature.WishList.Models
{
    public class WishListJsonResult : BaseJsonResult
    {
        [ScriptIgnore]
        private readonly ISearchManager _searchManager;

        public WishListJsonResult(IStorefrontContext storefrontContext, IContext context, ISearchManager searchManager)
        : base(context, storefrontContext)
        {
            _searchManager = searchManager;
        }

        public List<WishListLineJsonResult> WishListLines { get; set; } = new List<WishListLineJsonResult>();
        internal void Init()
        {
           
        }
    }
}