using System;
using System.Linq;
using System.Reflection;

using Geex.Core.SystemSettings.Domain;
using Geex.Core.UserManagement;
using Geex.Shared;
using Geex.Shared._ShouldMigrateToLib;
using Geex.Shared._ShouldMigrateToLib.Abstractions;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

using MoreLinq;
using StackExchange.Redis.Extensions.Core.Abstractions;
using Volo.Abp.Modularity;

namespace Geex.Core.SystemSettings
{
    [DependsOn(
    )]
    public class SystemSettingModule : GraphQLModule<UserManagementModule>
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddSingleton<ISettingManager, GeexSettingManager>(x =>
            {
                var modules = x.GetServices<GraphQLModule>()
                    .Select(y => y.GetType().Assembly).Distinct();
                var definitionTypes = modules
                    .SelectMany(y => y.DefinedTypes
                    .Where(z => z.BaseType == typeof(SettingDefinition)));
                var settingDefinitions =
                    definitionTypes.SelectMany(x => x.GetProperties(BindingFlags.Static | BindingFlags.Public).Where(y => y.DeclaringType.IsAssignableTo(x))).Select(x => x.GetValue(null)).Cast<SettingDefinition>();
                return new GeexSettingManager(x.GetRequiredService<IRedisDatabase>(), settingDefinitions);
            });
            base.ConfigureServices(context);
        }
    }
}
