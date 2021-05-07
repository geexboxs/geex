using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

using Geex.Shared._ShouldMigrateToLib;
using MoreLinq;

namespace Geex.Core.SystemSettings.Domain
{
    public interface ISettingManager
    {
        public async Task<Setting> SetAsync(IUpdateSettingParams updateSettingParams)
        {
            var definition = this.SettingDefinitions.FirstOrDefault(x => x.Name == updateSettingParams.Name);
            if (definition == default)
            {
                throw new GeexBusinessException(GeexExceptionType.NotFound, message: "setting name not exists.");
            }
            Setting setting = !updateSettingParams.ScopedKey.IsNullOrEmpty() ? Settings.FirstOrDefault(x => x.Scope == updateSettingParams.Scope && x.ScopedKey == updateSettingParams.ScopedKey) : Settings.First(x => x.Scope == updateSettingParams.Scope);
            if (setting == default)
            {
                setting = new Setting(definition.Name, updateSettingParams.Value, updateSettingParams.Scope, updateSettingParams.ScopedKey);
                this.Settings.Add(setting);
            }
            setting.Value = updateSettingParams.Value;
            return setting;
        }

        IReadOnlyList<SettingDefinition> SettingDefinitions { get; }
        ObservableCollection<Setting> Settings { get; }

        async Task<IEnumerable<Setting>> GetAllForUserAsync(ClaimsPrincipal identity)
        {
            var userSettings = this.Settings.Where(x => x.Scope == SettingScopeEnumeration.User && x.ScopedKey == identity.FindUserId());
            var globalSettings = this.Settings.Where(x => x.Scope == SettingScopeEnumeration.Global).ExceptBy(userSettings,x=> x.Name);
            return userSettings.Concat(globalSettings);
        }
        async Task<IEnumerable<Setting>> GetAllGlobalAsync()
        {
            return this.Settings.Where(x => x.Scope == SettingScopeEnumeration.Global);
        }

        async Task<Setting?> GetOrNullAsync(SettingDefinition settingDefinition, SettingScopeEnumeration? settingScope = default,
            string? scopedKey = default)
        {
            var filteredSettings = this.Settings.Where(x => x.Name == settingDefinition.Name);
            if (settingScope != default)
            {
                filteredSettings = filteredSettings.Where(x => x.Scope == settingScope);
            }

            if (scopedKey != default)
            {
                filteredSettings = filteredSettings.Where(x => x.ScopedKey == scopedKey);
            }
            return filteredSettings.FirstOrDefault();
        }
    }
}