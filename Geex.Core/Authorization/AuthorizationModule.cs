using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casbin.AspNetCore.Authorization;
using Geex.Core.Authorization.Casbin;
using Geex.Shared;
using Geex.Shared._ShouldMigrateToLib.Auth;
using Microsoft.AspNetCore.Builder;
using MongoDB.Entities;
using NetCasbin;
using Volo.Abp;
using Volo.Abp.AspNetCore;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace Geex.Core.Authorization
{
    [DependsOn(
        typeof(AbpAspNetCoreModule),
        typeof(AbpUnitOfWorkModule)
    )]
    public class AuthorizationModule:GraphQLModule<AuthorizationModule>
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            var services = context.Services;
            services.AddCasbinAuthorization(options =>
            {
                options.DefaultEnforcerFactory = model =>
                    new RbacEnforcer(new CasbinMongoAdapter(() => DB.Collection<CasbinRule>()));
            });
            base.PreConfigureServices(context);
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            app.UseCasbinAuthorization();
            base.OnApplicationInitialization(context);
        }
    }
}
