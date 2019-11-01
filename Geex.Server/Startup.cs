using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Geex.Server.Types;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Voyager;
using HotChocolate.Language;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Geex.Server
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication();
            services.AddGraphQL(provider =>
                SchemaBuilder.New()
                    .AddServices(provider)
                    .AddAuthorizeDirectiveType()
                    .AddQueryType<QueryType>()
                    .AddMutationType<MutationType>()
                    .AddSubscriptionType<SubscriptionType>()
                    .AddTypes(typeof(Startup).Assembly.DefinedTypes.Where(x => x.Namespace == $"{typeof(Startup).Namespace}.Types").ToArray())
                    .Create());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();

            app.UseGraphQL();
            app.UseVoyager();
            app.UsePlayground();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
