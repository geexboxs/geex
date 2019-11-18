using System;
using System.Linq;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Geex.Core;
using Geex.Core.UserManagement;
using Geex.Data;
using Geex.Shared;
using Geex.Shared._ShouldMigrateToLib;
using Geex.Shared.Roots.RootTypes;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Voyager;
using HotChocolate.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Geex.Server
{
    public class Startup
    {
        public Startup(IHostEnvironment env)
        {
            // In ASP.NET Core 3.0 `env` will be an IWebHostingEnvironment, not IHostingEnvironment.
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public ILifetimeScope AutofacContainer { get; set; }
        public IConfigurationRoot Configuration { get; set; }


        // This is the default if you don't have an environment specific method.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication();
            services.AddHealthChecks();
            AddIdentityServerWithAspNetIdentity(services);


            services.AddGeexGraphQL<AppModule>();
        }


        // This is the default if you don't have an environment specific method.
        public void ConfigureContainer(ContainerBuilder builder)
        {
        }

        // This is the default if you don't have an environment specific method.
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHealthChecks("/health-check");


            app.UseAuthentication();

            app.UseGeexGraphQL();
        }

        private void AddIdentityServerWithAspNetIdentity(IServiceCollection services)
        {
            services.AddIdentityServer(options =>
                {
                    options.Authentication.CookieLifetime = new TimeSpan(24, 0, 0);
                    options.Authentication.CookieSlidingExpiration = false;
                })
                .AddJwtBearerClientAuthentication()
                .AddDeveloperSigningCredential()
                // lulus:此处添加正式证书,不知道证书加密类型,未作处理
                //.AddSigningCredential()
                .AddMongoRepository()
                .AddRedirectUriValidator<RegexRedirectUriValidator>()
                .AddCorsPolicyService<RegexCorsPolicyService>();
            //bug :idsr的bug
            //services.Replace(ServiceDescriptor.Transient<ICorsPolicyService, RegexCorsPolicyService>());
        }

    }
}