using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using CommonServiceLocator;

using Geex.Shared._ShouldMigrateToLib;
using Geex.Shared._ShouldMigrateToLib.Abstractions;

using Microsoft.Extensions.DependencyInjection;

using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Geex.Core.SystemSettings.Domain
{
    public static class Extensions
    {
        public static async Task SetAsync(this ISettingManager settingManager, IUpdateSettingParams settingValue)
        {
            await settingManager.SetAsync(settingValue);
        }

        /// <summary>
        /// 获取当前生效的setting
        /// </summary>
        /// <typeparam name="TProviderType"></typeparam>
        /// <typeparam name="TValueType"></typeparam>
        /// <param name="settingManager"></param>
        /// <param name="settingName"></param>
        /// <returns></returns>
        public static async Task<Setting?> GetEffectiveAsync<TProviderType>(this ISettingManager settingManager, TProviderType settingName, ClaimsIdentity identity) where TProviderType : SettingDefinition
        {
            Setting setting;
            setting = await settingManager.GetOrNullAsync(settingName, SettingScopeEnumeration.User, identity.Claims.FirstOrDefault(x => x.Type == GeexClaimType.Sub).Value) ??
                      await settingManager.GetOrNullAsync(settingName, SettingScopeEnumeration.Global);
            return setting;
        }
    }

    public interface IUpdateSettingParams
    {
        /// <summary>
        /// setting名称
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// setting值
        /// </summary>
        public string Value { get; }
        /// <summary>
        /// 对应主键(租户id/用户id/...)
        /// </summary>
        public string ScopedKey { get; }
        /// <summary>
        /// setting提供方
        /// </summary>

        public SettingScopeEnumeration Scope { get; }
    }
}
