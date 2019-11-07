using System;
using System.Linq;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Geex.Core;
using Geex.Core.User;
using Geex.Data;
using Geex.Shared;
using Geex.Shared.Roots.RootTypes;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Voyager;
using HotChocolate.Types;
using IdentityServer4.AspNetIdentity;
using IdentityServer4.Services;
using Microex.All.EntityFramework;
using Microex.All.IdentityServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
            services.AddIdentity<User, Role>(
                    options =>
                    {
                        // Password settings
                        options.Password.RequireDigit = true;
                        options.Password.RequiredLength = 6;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireUppercase = false;
                        options.Password.RequireLowercase = false;

                        // Lockout settings
                        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                        options.Lockout.MaxFailedAccessAttempts = 10;
                        options.Lockout.AllowedForNewUsers = true;

                        // User settings
                        options.User.RequireUniqueEmail = false;
                    })
                .AddUserManager<MicroexUserManager<User>>()
                .AddSignInManager<MicroexSignInManager<User>>()
                .AddEntityFrameworkStores<GeexDbContext>()
                .AddDefaultTokenProviders();
            services.AddIdentityServer(options =>
            {
                options.Authentication.CookieLifetime = new TimeSpan(24, 0, 0);
                options.Authentication.CookieSlidingExpiration = false;
            })
                .AddJwtBearerClientAuthentication()
                .AddDeveloperSigningCredential()
                // lulus:此处添加正式证书,不知道证书加密类型,未作处理
                //.AddSigningCredential()
                .AddConfigurationStore<GeexDbContext>()
                .AddOperationalStore<GeexDbContext>()
                .AddProfileService<ProfileService<User>>()
                .AddRedirectUriValidator<RegexRedirectUriValidator>()
                .AddCorsPolicyService<RegexCorsPolicyService>()
                .AddConfigurationStoreCache()
                .AddAspNetIdentity<User>();
            //bug :idsr的bug
            //services.Replace(ServiceDescriptor.Transient<ICorsPolicyService, RegexCorsPolicyService>());


            services.AddDbContext<IdentityServerDbContext<User>>(options =>
             {
                 options.UseSqlServer(Configuration.GetConnectionString("Default"));
             });

            services.AddGeexGraphQL<AppModule>();
        }
        // This is the default if you don't have an environment specific method.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Add things to the Autofac ContainerBuilder.
        }

        // This is the default if you don't have an environment specific method.
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            AutofacContainer = app.ApplicationServices.GetAutofacRoot();
            app.EnsurePredefinedIdentityServerConfigs<IdentityServerDbContext<User>, User>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHealthChecks("/health-check");


            app.UseAuthentication();

            app.UseGeexGraphQL();
        }
    }
}