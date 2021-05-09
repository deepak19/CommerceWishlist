// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigureServiceApiBlock.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2017
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Website.Plugin.WishLists
{
    using System.Threading.Tasks;
    using Website.Plugin.WishLists.Components;
    using Website.Plugin.WishLists.Entities;
    using Website.Plugin.WishLists.Models;
    using Microsoft.AspNetCore.OData.Builder;

    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.Core.Commands;
    using Sitecore.Framework.Conditions;
    using Sitecore.Framework.Pipelines;

    /// <summary>
    /// Defines a block which configures the OData model
    /// </summary>
    /// <seealso>
    ///     <cref>
    ///         Sitecore.Framework.Pipelines.PipelineBlock{Microsoft.AspNetCore.OData.Builder.ODataConventionModelBuilder,
    ///         Microsoft.AspNetCore.OData.Builder.ODataConventionModelBuilder,
    ///         Sitecore.Commerce.Core.CommercePipelineExecutionContext}
    ///     </cref>
    /// </seealso>
    [PipelineDisplayName("Website.Plugin.WishLists.ConfigureServiceApiBlock")]
    public class ConfigureServiceApiBlock : PipelineBlock<ODataConventionModelBuilder, ODataConventionModelBuilder, CommercePipelineExecutionContext>
    {
        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="arg">
        /// The argument.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The <see cref="ODataConventionModelBuilder"/>.
        /// </returns>
        public override Task<ODataConventionModelBuilder> Run(ODataConventionModelBuilder arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull($"{this.Name}: The argument cannot be null.");
            arg.AddEntityType(typeof(WishList));
            arg.EntitySet<WishList>("WishLists");

            ActionConfiguration configuration1 = arg.Action("CreateWishList");
            configuration1.Parameter<string>("wishListName");
            configuration1.ReturnsFromEntitySet<CommerceCommand>("Commands");

            ActionConfiguration configuration2 = arg.Action("DeleteWishList");
            configuration2.Parameter<string>("wishListName");
            configuration2.ReturnsFromEntitySet<CommerceCommand>("Commands");

            ActionConfiguration configuration3 = arg.Action("AddLinesToWishList");
            configuration3.Parameter<string>("wishListName");
            configuration3.CollectionParameter<WishListLineRequest>("Lines");
            configuration3.ReturnsFromEntitySet<CommerceCommand>("Commands");

            ActionConfiguration configuration4 = arg.Action("RemoveWishListLines");
            configuration4.Parameter<string>("wishListName");
            configuration4.CollectionParameter<WishListLineRequest>("Lines");
            configuration4.ReturnsFromEntitySet<CommerceCommand>("Commands");

            ActionConfiguration configuration5 = arg.Action("GetWishList");
            configuration5.Parameter<string>("Id");
            configuration5.ReturnsFromEntitySet<CommerceCommand>("Commands");

            ActionConfiguration configuration6 = arg.Action("GetWishLists");
            configuration6.ReturnsFromEntitySet<CommerceCommand>("Commands");

            return Task.FromResult(arg);
        }
    }
}
