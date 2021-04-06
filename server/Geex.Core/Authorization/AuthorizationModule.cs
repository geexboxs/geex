﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Geex.Core.Authorization.Casbin;
using Geex.Shared;
using Geex.Shared._ShouldMigrateToLib.Auth;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Entities;

using NetCasbin;

using Volo.Abp;
using Volo.Abp.AspNetCore;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace Geex.Core.Authorization
{
    [DependsOn(
    )]
    public class AuthorizationModule : GraphQLModule<AuthorizationModule>
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var services = context.Services;
            services.AddCasbinAuthorization();
            base.ConfigureServices(context);
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            app.UseAuthorization();
            base.OnApplicationInitialization(context);
        }
    }
}