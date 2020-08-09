using Autofac.Extras.CommonServiceLocator;

using CommonServiceLocator;

using Geex.Shared._ShouldMigrateToLib.Auth;
using Geex.Shared._ShouldMigrateToLib;
using IdentityServer4.MongoDB.Mappers;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using MongoDB.Driver;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.AspNetCore;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.TenantManagement;
using ApiResource = IdentityServer4.MongoDB.Entities.ApiResource;
using Client = IdentityServer4.MongoDB.Entities.Client;
using IdentityResource = IdentityServer4.MongoDB.Entities.IdentityResource;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Geex.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Humanizer;
using Geex.Core.Users;
using Geex.Contract;

namespace Geex.Core
{
    [DependsOn(
        typeof(UserModule),
        typeof(AbpAccountApplicationModule),
        typeof(ContractsModule),
        typeof(AbpIdentityApplicationModule),
        typeof(AbpPermissionManagementApplicationModule),
        typeof(AbpTenantManagementApplicationModule),
        typeof(AbpFeatureManagementApplicationModule)
        )]
    public class AppModule : GraphQLEntryModule<AppModule>
    {
        private IWebHostEnvironment _env;
        private IConfiguration _configuration;

        public override void OnApplicationInitialization(
            ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            this._env = context.GetEnvironment();
            this._configuration = context.GetConfiguration();

            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(app.ApplicationServices.GetAutofacRoot()));
            InitializeIdentityServerDatabase(app);
            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
            });

            app.UseHealthChecks("/health-check");

            //app.UseAuthentication();
            app.UseIdentityServer();

            app.UseGeexGraphQL();
        }

        private static void InitializeIdentityServerDatabase(IApplicationBuilder app)
        {

            bool createdNewRepository = false;
            var repository = app.ApplicationServices.GetService<IMongoDatabase>().GetCollection<Client>(nameof(Client).Humanize().Pluralize());
            if (repository.AsQueryable().Any())
            {
                return;
            }

            //  --Client
            IdentityServerSeedConfig identityServerSeedConfig = app.ApplicationServices.GetService<IdentityServerSeedConfig>();
            foreach (var client in identityServerSeedConfig.Clients)
            {
                app.ApplicationServices.GetService<IMongoDatabase>().GetCollection<Client>(nameof(Client).Humanize().Pluralize()).InsertOne(client.ToEntity());
            }
            foreach (var res in identityServerSeedConfig.IdentityResources)
            {
                app.ApplicationServices.GetService<IMongoDatabase>().GetCollection<IdentityResource>(nameof(IdentityResource).Humanize().Pluralize()).InsertOne(res.ToEntity());
            }
            foreach (var api in identityServerSeedConfig.ApiResources)
            {
                app.ApplicationServices.GetService<IMongoDatabase>().GetCollection<ApiResource>(nameof(ApiResource).Humanize().Pluralize()).InsertOne((api.ToEntity()));
            }
            createdNewRepository = true;


            // If it's a new Repository (database), need to restart the website to configure Mongo to ignore Extra Elements.
            if (createdNewRepository)
            {
                var newRepositoryMsg = $"Mongo Repository created/populated! Please restart you website, so Mongo driver will be configured  to ignore Extra Elements - e.g. IdentityServer \"_id\" ";
                throw new Exception(newRepositoryMsg);
            }
        }
    }
}
