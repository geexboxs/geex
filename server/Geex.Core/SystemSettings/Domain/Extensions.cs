using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using CommonServiceLocator;

using Geex.Core.SystemSettings.Domain;

using Volo.Abp.MultiTenancy;
using Volo.Abp.SettingManagement;
using Volo.Abp.Users;

using JsonSerializer = System.Text.Json.JsonSerializer;

// ReSharper disable once CheckNamespace
namespace Volo.Abp.Settings
{
    public static class Extensions
    {
        public static async Task<T> GetOrNullAsync<T>(this ISettingValueProvider @this, SettingDefinition setting)
        {
            var settingValue = await @this.GetOrNullAsync(setting);
            return settingValue.IsNullOrEmpty() ? default : JsonSerializer.Deserialize<T>(settingValue);
        }


        public static async Task SetAsync(this ISettingManager settingManager, ISettingValue settingValue)
        {
            await settingManager.SetAsync(settingValue.Name, settingValue.Value,
                settingValue.SettingProvider.Value, settingValue.ProviderKey);
        }

        /// <summary>
        /// 获取当前生效的setting
        /// </summary>
        /// <typeparam name="TProviderType"></typeparam>
        /// <typeparam name="TValueType"></typeparam>
        /// <param name="settingManager"></param>
        /// <param name="settingName"></param>
        /// <returns></returns>
        public static async Task<TValueType> GetEffectiveAsync<TProviderType, TValueType>(this ISettingManager settingManager, TProviderType settingName) where TProviderType : RmsSettingsBase
        {

            string settingStr;
            settingStr = await settingManager.GetOrNullAsync(settingName, SettingProviderEnumeration.User, ServiceLocator.Current.GetInstance<ICurrentUser>()?.Id.ToString(), false);
            if (settingStr == default)
            {
                settingStr = await settingManager.GetOrNullAsync(settingName, SettingProviderEnumeration.Tenant, ServiceLocator.Current.GetInstance<ICurrentTenant>()?.Id.ToString());
            }
            return JsonSerializer.Deserialize<TValueType>(settingStr);
        }
    }

    public interface ISettingValue
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
        public string ProviderKey { get; }
        /// <summary>
        /// setting提供方
        /// </summary>

        public SettableSettingProviderEnumeration SettingProvider { get; }
    }
}
