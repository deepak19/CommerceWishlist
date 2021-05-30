using Website.Plugin.WishLists.Components;
using Microsoft.AspNetCore.OData.Builder;
using Sitecore.Commerce.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Website.Plugin.WishLists.Entities
{
    public class WishList : CommerceEntity
    {
        public string ShopName { get; set; }
        public string CustomerId { get; set; }
        //public string UserId { get; set; }
        public bool IsFavorite { get; set; }

        [Contained]
        public IList<WishListComponent> Lines { get; set; }

        public WishList(string name)
        {
            Name = name;
            Lines = new List<WishListComponent>();
        }

    }
}
