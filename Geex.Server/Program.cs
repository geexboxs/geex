using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

using Autofac.Extensions.DependencyInjection;

using Geex.Core;
using Geex.Data;
using Geex.Shared;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Geex.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var hmac = new HMACSHA256(Convert.FromBase64String("test"));
            var a = hmac;
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureGeexServer<AppModule>();
    }
}
