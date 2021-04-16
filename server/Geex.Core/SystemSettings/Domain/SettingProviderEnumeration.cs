using Geex.Shared._ShouldMigrateToLib.Abstractions;

using Volo.Abp.Settings;

namespace Geex.Core.SystemSettings.Domain
{
    public class SettingProviderEnumeration : Enumeration<SettingProviderEnumeration, string>
    {

        public SettingProviderEnumeration(string name, string value) : base(name, value)
        {
        }

        /// <summary>
        /// 默认值
        /// </summary>
        public static readonly SettingProviderEnumeration DefaultValue = new(nameof(DefaultValue), DefaultValueSettingValueProvider.ProviderName);
        /// <summary>
        /// 基础配置
        /// </summary>
        public static readonly SettingProviderEnumeration Configuration = new(nameof(Configuration), ConfigurationSettingValueProvider.ProviderName);
        /// <summary>
        /// 全局运行时
        /// </summary>
        public static readonly SettingProviderEnumeration Global = new(nameof(Global), GlobalSettingValueProvider.ProviderName);
        /// <summary>
        /// 租户级
        /// </summary>
        public static readonly SettingProviderEnumeration Tenant = new(nameof(Tenant), TenantSettingValueProvider.ProviderName);
        /// <summary>
        /// 用户级
        /// </summary>
        public static readonly SettingProviderEnumeration User = new(nameof(User), UserSettingValueProvider.ProviderName);
    }

    public class SettableSettingProviderEnumeration : Enumeration<SettableSettingProviderEnumeration, string>
    {
        public SettableSettingProviderEnumeration(string name, string value) : base(name, value)
        {
        }

        /// <summary>
        /// 全局运行时
        /// </summary>
        public static readonly SettableSettingProviderEnumeration Global = new(nameof(Global), SettingProviderEnumeration.Global.Value);
        /// <summary>
        /// 租户级
        /// </summary>
        public static readonly SettableSettingProviderEnumeration Tenant = new(nameof(Tenant), SettingProviderEnumeration.Tenant.Value);
        /// <summary>
        /// 用户级
        /// </summary>
        public static readonly SettableSettingProviderEnumeration User = new(nameof(User), SettingProviderEnumeration.User.Value);
    }
}
