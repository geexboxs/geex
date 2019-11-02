using System.Linq;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Geex.Core;
using Geex.Core.User;
using Geex.Shared;
using Geex.Shared.Roots.RootTypes;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Voyager;
using HotChocolate.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            
            services.AddGraphQLEntryModule<AppModule>();
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

            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            app.UseAuthentication();

            app.UseGraphQL();
            app.UseVoyager();
            app.UsePlayground();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello World!"); });
            });
        }
    }
}