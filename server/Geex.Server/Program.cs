using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.CommonServiceLocator;

using CommonServiceLocator;

using Geex.Core;
using Geex.Data;
using Geex.Shared;
using Geex.Shared.Types;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using Volo.Abp.Autofac;

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
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterBuildCallback(x =>
                        ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(x)));
            Console.WriteLine(containerBuilder.GetHashCode());
            return Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AbpAutofacServiceProviderFactory(containerBuilder))
                .ConfigureLogging((ctx, builder) =>
                {
                    if (ctx.Configuration.GetValue<bool>("Logging:RollingFile:Enabled"))
                    {
                        builder.AddRollingFile();
                    }
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureServices((_, services) =>
                    {
                        services.AddObjectAccessor(containerBuilder);
                        services.AddGeexGraphQL<AppModule>();
                    });
                    webBuilder.Configure((webHostBuilderContext, app) => { app.InitializeApplication(); });
                });
        }
    }
}
