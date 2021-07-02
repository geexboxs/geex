using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Geex.Common.Messaging.Api;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using GeexBox.ElasticSearch.Zero.Logging.Elasticsearch;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Geex.Common.Messaging
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory(x =>
                {
                    x.RegisterBuildCallback(a =>
                    {
                        var csl = new AutofacServiceLocator(a);
                        ServiceLocator.SetLocatorProvider(() => csl);
                    });
                }))
                .ConfigureLogging((ctx, builder) =>
                {
                    if (ctx.Configuration.GetSection("Logging:Elasticsearch").GetChildren().Any())
                    {
                        builder.AddElasticsearch();
                    }
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureServices((_, services) =>
                    {
                        services.AddApplication<AppModule>();
                    });
                    webBuilder.Configure((webHostBuilderContext, app) => { app.InitializeApplication(); });
                });
    }
}
