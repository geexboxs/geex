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
using Geex.Core.Authorization.Casbin;
using Humanizer;
using IdentityServer4.MongoDB.Entities;
using IdentityServer4.MongoDB.Mappers;
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
            var app = context.GetApplicationBuilder();
        }

        public override void PostConfigureServices(ServiceConfigurationContext context)
        {
            base.PostConfigureServices(context);
            var env = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();
            var services = context.Services;

            services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddCasbinAuthorization();
            services.AddScoped<GeexCookieAuthenticationEvents>();
            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.EventsType = typeof(GeexCookieAuthenticationEvents);
                })
                    .AddIdentityServerAuthentication(
                        options =>
                        {
                            if (env.IsDevelopment())
                            {
                                IdentityModelEventSource.ShowPII = true;
                            }
                            options.ApiName = env.ApplicationName;
                            options.Authority = $"http://{Environment.GetEnvironmentVariable("HOST_NAME")}";
                            options.ApiSecret = env.ApplicationName;
                            options.RequireHttpsMetadata = false;
                            options.JwtBearerEvents = new JwtBearerEvents
                            {
                                OnChallenge = context =>
                                {
                                    return Task.CompletedTask;
                                },
                                OnTokenValidated = ContextBoundObject =>
                                {
                                    return Task.CompletedTask;
                                },
                                OnMessageReceived = context =>
                                {
                                    return Task.CompletedTask;
                                },
                                OnAuthenticationFailed = context =>
                                {
                                    var te = context.Exception;
                                    return Task.CompletedTask;
                                }
                            };
                        });

            services.AddHealthChecks();

            services.AddIdentityServer(options =>
            {
                options.Authentication.CookieLifetime = new TimeSpan(24, 0, 0);
                options.Authentication.CookieSlidingExpiration = false;
            })
                .AddJwtBearerClientAuthentication()
                .AddDeveloperSigningCredential()
                .AddResourceOwnerValidator<GeexPasswordValidator>()
                // lulus:此处添加正式证书,不知道证书加密类型,未作处理
                //.AddSigningCredential()
                .AddMongoRepository(configuration.GetConnectionString("Geex"))
                .AddRedirectUriValidator<RegexRedirectUriValidator>()
                .AddCorsPolicyService<RegexCorsPolicyService>();
            services.AddSingleton<IdentityServerSeedConfig>();
            //bug :idsr的bug
            //services.Replace(ServiceDescriptor.Transient<ICorsPolicyService, RegexCorsPolicyService>());
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            base.ConfigureServices(context);
            
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            base.OnApplicationInitialization(context);
            var app = context.GetApplicationBuilder();
            app.UseIdentityServer();
        }
    }
}
