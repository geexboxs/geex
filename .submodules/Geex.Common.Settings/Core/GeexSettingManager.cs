using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Claims;
using System.Threading.Tasks;

using Geex.Common.Abstractions;
using Geex.Common.Abstractions.Enumerations;
using Geex.Common.Settings.Abstraction;

using Microsoft.Extensions.Configuration;

using MoreLinq;

using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Geex.Common.Settings.Core
{
    public class GeexSettingManager
    {
        private IRedisDatabase _redisClient;

        public GeexSettingManager(IRedisDatabase redisClient, IEnumerable<GeexModule> modules)
        {
            _redisClient = redisClient;
            var definitionTypes = modules
                    .Select(y => y.GetType().Assembly).Distinct()
                    .SelectMany(y => y.DefinedTypes
                    .Where(z => z.BaseType == typeof(SettingDefinition)));
            var settingDefinitions =
                definitionTypes.SelectMany(x => x.GetProperties(BindingFlags.Static | BindingFlags.Public).Where(y => y.DeclaringType.IsAssignableTo(x))).Select(x => x.GetValue(null)).Cast<SettingDefinition>();
            SettingDefinitions = new ReadOnlyCollection<SettingDefinition>(settingDefinitions.ToArray());
            var defaultSettings = SettingDefinitions.Select(x => new Setting(x, x.DefaultValue, SettingScopeEnumeration.Global));
            Settings = new ObservableCollection<Setting>(defaultSettings);
            var existedSettings = _redisClient.GetAllNamedAsync<Setting>().Result.Select(x => x.Value);
            var userSettings = existedSettings.Where(x => x.Scope == SettingScopeEnumeration.User);
            var overrideSettings = existedSettings.Where(x => x.Scope == SettingScopeEnumeration.Global);
            var overrideSettingNames = overrideSettings.Select(x => x.Name);
            Settings.ReplaceWhile(x => overrideSettingNames.Contains(x.Name), origin => overrideSettings.First(x => x.Name == origin.Name));
            foreach (var userSetting in userSettings)
            {
                this.Settings.Add(userSetting);
            }
            this.Settings.CollectionChanged += async (o, args) => await this.Settings_CollectionChanged(o, args);
        }

        private async Task Settings_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (Setting newItem in e.NewItems)
                    {
                        await _redisClient.SetNamedAsync(newItem);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (Setting newItem in e.OldItems)
                    {
                        await _redisClient.RemoveNamedAsync<Setting>(newItem.GetId());
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    await _redisClient.RemoveAllNamedAsync<Setting>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public IReadOnlyList<SettingDefinition> SettingDefinitions { get; }
        public ObservableCollection<Setting> Settings { get; private set; }

        /// <summary>
        /// 获取当前生效的setting
        /// </summary>
        /// <typeparam name="TProviderType"></typeparam>
        /// <param name="settingName"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        public async Task<Setting?> GetEffectiveAsync<TProviderType>(TProviderType settingName, ClaimsIdentity identity) where TProviderType : SettingDefinition
        {
            Setting setting;
            setting = await this.GetOrNullAsync(settingName, SettingScopeEnumeration.User, identity.Claims.FirstOrDefault(x => x.Type == GeexClaimType.Sub).Value) ??
                      await this.GetOrNullAsync(settingName, SettingScopeEnumeration.Global);
            return setting;
        }
        public async Task<Setting> SetAsync(SettingDefinition settingDefinition, SettingScopeEnumeration scope, string? scopedKey, string? value)
        {
            var definition = this.SettingDefinitions.FirstOrDefault(x => x.Name == settingDefinition);
            if (definition == default)
            {
                throw new BusinessException(GeexExceptionType.NotFound, message: "setting name not exists.");
            }
            var setting = scopedKey.IsNullOrEmpty() ? Settings.FirstOrDefault(x => x.Scope == scope && x.ScopedKey == scopedKey) : Settings.First(x => x.Scope == scope);
            if (setting != default)
            {
                this.Settings.Remove(setting);
            }
            setting = new Setting(definition, value, scope, scopedKey);
            this.Settings.Add(setting);
            return setting;
        }

        public async Task<IEnumerable<Setting>> GetAllForCurrentUserAsync(ClaimsPrincipal identity)
        {
            var userSettings = this.Settings.Where(x => x.Scope == SettingScopeEnumeration.User && x.ScopedKey == identity.FindUserId());
            var globalSettings = this.Settings.Where(x => x.Scope == SettingScopeEnumeration.Global).ExceptBy(userSettings, x => x.Name);
            return userSettings.Concat(globalSettings);
        }
        public async Task<IEnumerable<Setting>> GetGlobalSettingsAsync()
        {
            return this.Settings.Where(x => x.Scope == SettingScopeEnumeration.Global);
        }

        public async Task<IEnumerable<Setting>> GetUserSettingsAsync(ClaimsPrincipal identity)
        {
            return this.Settings.Where(x => x.Scope == SettingScopeEnumeration.User && x.ScopedKey == identity.FindUserId());
        }

        public async Task<Setting?> GetOrNullAsync(SettingDefinition settingDefinition, SettingScopeEnumeration settingScope = default,
            string? scopedKey = default)
        {
            if (settingScope == default)
            {
                var filteredSettings = this.Settings.Where(x => x.Name == settingDefinition.Name)
                    .WhereIf(!scopedKey.IsNullOrEmpty(), x => x.ScopedKey == scopedKey)
                    .WhereIf(scopedKey.IsNullOrEmpty(), x => x.ScopedKey == default);

                return filteredSettings.OrderByDescending(x => x.Scope.Priority).FirstOrDefault();
            }
            else
            {
                var filteredSettings = this.Settings.Where(x => x.Name == settingDefinition.Name && x.Scope == settingScope)
                    .WhereIf(!scopedKey.IsNullOrEmpty(), x => x.ScopedKey == scopedKey);

                return filteredSettings.SingleOrDefault();
            }
        }
    }
}