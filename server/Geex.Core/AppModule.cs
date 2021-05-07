using System.Threading.Tasks;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Volo.Abp;
using Volo.Abp.Modularity;
using Autofac.Extensions.DependencyInjection;
using Geex.Core.Authentication;
using Geex.Core.Authorization;
using Geex.Core.Captcha;
using Geex.Core.Notification;
using Geex.Core.SystemSettings;
using Geex.Core.UserManagement;
using Microsoft.AspNetCore.Http;
using Geex.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Geex.Core.Users;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MongoDB.Entities;
using StackExchange.Redis.Extensions.Core;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Validation;

namespace Geex.Core
{
    [DependsOn(
        typeof(AuthenticationModule),
        typeof(NotificationModule),
        typeof(AuthorizationModule),
        typeof(CaptchaModule),
        typeof(SystemSettingModule),
        typeof(UserManagementModule)
        )]
    public class AppModule : GraphQLEntryModule<AppModule>
    {
        private IWebHostEnvironment _env;
        private IConfiguration _configuration;


        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            this._env = context.Services.GetSingletonInstance<IWebHostEnvironment>();
            this._configuration = context.Services.GetConfiguration();
            //this.Configure<AbpAuditingOptions>((Action<AbpAuditingOptions>)(options => options.Contributors.Add((AuditLogContributor)new AspNetCoreAuditLogContributor())));
            context.Services.AddStackExchangeRedisExtensions();
            context.Services.AddMediatR(typeof(AppModule));
            context.Services.AddHttpContextAccessor();
            context.Services.AddObjectAccessor<IApplicationBuilder>();
            //context.Services.Replace(ServiceDescriptor.Transient<IOptionsFactory<RequestLocalizationOptions>, AbpRequestLocalizationOptionsFactory>());


            context.Services.AddHealthChecks();
            context.Services.AddCors(options =>
            {
                if (_env.IsDevelopment())
                {
                    options.AddDefaultPolicy(x => x.SetIsOriginAllowed(x => true).AllowAnyHeader().AllowAnyMethod().AllowCredentials());
                }
            });
            base.ConfigureServices(context);
        }

        

        public override void OnApplicationInitialization(
            ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            this._env = context.GetEnvironment();
            this._configuration = context.GetConfiguration();

            app.UseCors();

            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
            });


            app.UseHealthChecks("/health-check");

            app.UseGeexGraphQL();
        }
    }
}
