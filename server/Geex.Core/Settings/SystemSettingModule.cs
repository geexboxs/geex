using System;
using System.Linq;
using System.Reflection;
using Geex.Core.Settings.Domain;
using Geex.Core.UserManagement;
using Geex.Shared;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis.Extensions.Core.Abstractions;
using Volo.Abp.Modularity;

namespace Geex.Core.Settings
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
