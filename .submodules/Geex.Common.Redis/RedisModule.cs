using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Geex.Common.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis.Extensions.Core;
using Volo.Abp.Modularity;

namespace Geex.Common.Redis
{
    public class RedisModule : GeexModule<RedisModule>
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            base.PreConfigureServices(context);
            context.Services.PreConfigure<RedisModuleOptions>(options =>
            {
                Configuration.GetSection(nameof(RedisModuleOptions)).Bind(options);
            });
        }
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddStackExchangeRedisExtensions();
            base.ConfigureServices(context);
        }
    }
}
