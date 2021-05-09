using Website.Plugin.WishLists.Components;
using Website.Plugin.WishLists.Entities;
using Sitecore.Commerce.Core;
using System.Collections.Generic;

namespace Website.Plugin.WishLists.Pipelines.Arguments
{
    public class WishListLinesArg : PipelineArgument
    {
        public WishList WishList { get; set; }
        public List<WishListComponent> Lines { get; set; }

        public WishListLinesArg()
        {
            Lines = new List<WishListComponent>();
        }
    }

}