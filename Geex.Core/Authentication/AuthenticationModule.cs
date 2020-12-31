using Geex.Shared;
using Geex.Shared._ShouldMigrateToLib.Auth;
using Geex.Shared._ShouldMigrateToLib;

using Microsoft.AspNetCore.Authentication.Cookies;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Text;

using Volo.Abp.Modularity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Threading.Tasks;
using Geex.Core.Authentication.Domain;
using Humanizer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Volo.Abp;
using Volo.Abp.AspNetCore;
using Volo.Abp.Uow;

namespace Geex.Core.Authentication
{
    [DependsOn(
        typeof(AbpAspNetCoreModule),
        typeof(AbpUnitOfWorkModule)
    )]
    public class AuthenticationModule : GraphQLModule<AuthenticationModule>
    {
        public override void OnPreApplicationInitialization(ApplicationInitializationContext context)
        {
            base.OnPreApplicationInitialization(context);
        }

        public override void PostConfigureServices(ServiceConfigurationContext context)
        {
            base.PostConfigureServices(context);
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var services = context.Services;
            services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(x =>
                {
                    if (x.Events != null)
                    {
                        x.Events.OnMessageReceived = receivedContext => { return Task.CompletedTask; };
                        x.Events.OnAuthenticationFailed = receivedContext => { return Task.CompletedTask; };
                        x.Events.OnChallenge = receivedContext => { return Task.CompletedTask; };
                        x.Events.OnForbidden = receivedContext => { return Task.CompletedTask; };
                        x.Events.OnTokenValidated = receivedContext => { return Task.CompletedTask; };
                    };
                });
            var configuration = context.Services.GetConfiguration();
            services.AddSingleton(new UserTokenGenerateOptions(configuration.GetAppName(), "123456789012345678901234", TimeSpan.FromMinutes(10)));
            base.ConfigureServices(context);

        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            app.UseAuthentication();
            base.OnApplicationInitialization(context);
        }
    }
}
