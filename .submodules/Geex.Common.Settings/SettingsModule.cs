using System;
using System.Linq;
using System.Reflection;

using Geex.Common.Abstractions;
using Geex.Common.Redis;
using Geex.Common.Settings.Abstraction;
using Geex.Common.Settings.Core;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using StackExchange.Redis.Extensions.Core.Abstractions;

using Volo.Abp.Modularity;

namespace Geex.Common.Settings
{
    [DependsOn(
        typeof(RedisModule)
    )]
    public class SettingsModule : GeexModule<SettingsModule>
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            base.ConfigureServices(context);
        }

        public override void PostConfigureServices(ServiceConfigurationContext context)
        {

            context.Services.AddSingleton<GeexSettingManager>();
            base.PostConfigureServices(context);
        }
    }
}
