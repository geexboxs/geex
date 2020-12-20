using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommonServiceLocator;

using Geex.Core;

using Humanizer;

using IdentityServer4.MongoDB.Entities;
using IdentityServer4.MongoDB.Mappers;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MongoDB.Driver;
using MongoDB.Entities;

namespace Geex.Data.Migrations
{
    public class _0001_init : IMigration
    {
        public async Task UpgradeAsync()
        {
            this.InitializeIdentityServerCollections();
        }
        private void InitializeIdentityServerCollections()
        {
            bool createdNewRepository = false;
            var cfg = ServiceLocator.Current.GetInstance<IConfiguration>();
            var repository = DB.Database(cfg.GetAppName()).GetCollection<Client>(nameof(Client).Humanize().Pluralize());
            if (repository.AsQueryable().Any())
            {
                return;
            }

            //  --client
            IdentityServerSeedConfig identityServerSeedConfig = ServiceLocator.Current.GetInstance<IdentityServerSeedConfig>();
            foreach (var client in identityServerSeedConfig.Clients)
            {
                DB.Database(cfg.GetAppName()).GetCollection<Client>(nameof(Client).Humanize().Pluralize()).InsertOne(client.ToEntity());
            }
            // identityResource
            foreach (var res in identityServerSeedConfig.IdentityResources)
            {
                DB.Database(cfg.GetAppName()).GetCollection<IdentityResource>(nameof(IdentityResource).Humanize().Pluralize()).InsertOne(res.ToEntity());
            }
            // apiResource
            foreach (var api in identityServerSeedConfig.ApiResources)
            {
                DB.Database(cfg.GetAppName()).GetCollection<ApiResource>(nameof(ApiResource).Humanize().Pluralize()).InsertOne((api.ToEntity()));
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
