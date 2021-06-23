using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Threading.Tasks;

using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using Geex.Common;
using Geex.Core;
using Geex.Shared;
using GeexBox.ElasticSearch.Zero.Logging.Elasticsearch;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.DependencyInjection;


namespace Geex.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
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
}
