// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigureSitecore.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2017
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Website.Plugin.WishLists
{
    using System.Reflection;
    using Website.Plugin.WishLists.Pipelines;
    using Website.Plugin.WishLists.Pipelines.Blocks;
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.Commerce.Core;
    using Sitecore.Framework.Configuration;
    using Sitecore.Framework.Pipelines.Definitions.Extensions;

    /// <summary>
    /// The configure sitecore class.
    /// </summary>
    public class ConfigureSitecore : IConfigureSitecore
    {
        /// <summary>
        /// The configure services.
        /// </summary>
        /// <param name="services">
        /// The services.
        /// </param>
        public void ConfigureServices(IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.RegisterAllPipelineBlocks(assembly);

            services.Sitecore().Pipelines(config => config
            .AddPipeline<IPersistWishListPipeline, PersistWishListPipeline>(
                c =>
                {
                    c.Add<PersistWishListBlock>();
                }
                )
            .AddPipeline<ICreateWishListPipeline, CreateWishListPipeline>(
                c =>
                {
                    c.Add<CreateWishListBlock>();
                    c.Add<IPersistWishListPipeline>();
                }
                )
                .AddPipeline<IDeleteWishListPipeline, DeleteWishListPipeline>(
                c => 
                {
                    c.Add<DeleteWishListBlock>();
                }
                )
           .AddPipeline<IAddLinesToWishListPipeline, AddLinesToWishListPipeline>(
                c =>
                {
                    c.Add<AddLinesToWishListBlock>();
                    c.Add<IPersistWishListPipeline>();
                }
                )
           .AddPipeline<IRemoveWishlistLinesPipeline, RemoveWishlistLinesPipeline>(
                c => 
                {
                    c.Add<RemoveWishListLinesBlock>();
                    c.Add<IPersistWishListPipeline>();
                }
                )

               .ConfigurePipeline<IConfigureServiceApiPipeline>(configure => configure.Add<ConfigureServiceApiBlock>()));

            services.RegisterAllCommands(assembly);
        }
    }
}