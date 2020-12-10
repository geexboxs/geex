using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Volo.Abp;
using Volo.Abp.AspNetCore;
using Volo.Abp.Modularity;
using Autofac.Extensions.DependencyInjection;
using Geex.Core.Authentication;
using Geex.Core.UserManagement;
using Microsoft.AspNetCore.Http;
using Geex.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Geex.Core.Users;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Uow;

namespace Geex.Core
{
    [DependsOn(
        typeof(AbpAspNetCoreModule),
        typeof(AbpUnitOfWorkModule),
        typeof(AuthenticationModule),
        typeof(UserManagementModule)
        )]
    public class AppModule : GraphQLEntryModule<AppModule>
    {
        private IWebHostEnvironment _env;
        private IConfiguration _configuration;


        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHealthChecks();
            base.ConfigureServices(context);
        }

        public override void OnApplicationInitialization(
            ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            this._env = context.GetEnvironment();
            this._configuration = context.GetConfiguration();

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
