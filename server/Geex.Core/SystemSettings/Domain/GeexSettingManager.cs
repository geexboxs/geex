using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

using Geex.Shared._ShouldMigrateToLib;

using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Core.Abstractions;


namespace Geex.Core.SystemSettings.Domain
{
    public class GeexSettingManager : ISettingManager
    {
        private readonly IRedisDatabase _redisClient;

        public GeexSettingManager(IRedisDatabase redisClient, IEnumerable<SettingDefinition> settingDefinitions)
        {
            _redisClient = redisClient;
            SettingDefinitions = new ReadOnlyCollection<SettingDefinition>(settingDefinitions.ToArray());
            var globalSettings = SettingDefinitions.Select(x => new Setting(x.Name, x.DefaultValue, SettingScopeEnumeration.Global));
            var existedSettings = redisClient.GetAllNamedAsync<Setting>().Result.Select(x => x.Value);
            var userSettings = existedSettings.Where(x => x.Scope == SettingScopeEnumeration.User);
            var overrideSettings = existedSettings.Where(x => x.Scope == SettingScopeEnumeration.Global);
            foreach (var overrideSetting in overrideSettings)
            {
                var globalSetting = globalSettings.FirstOrDefault(x => x.Name == overrideSetting.Name);
                if (globalSetting != null)
                    globalSetting.Value = overrideSetting.Value;
            }
            Settings = new ObservableCollection<Setting>(globalSettings.Concat(userSettings));
            this.Settings.CollectionChanged += async (o, args) => await this.Settings_CollectionChanged(o, args);
        }


        private async Task Settings_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Replace:
                    foreach (Setting newItem in e.NewItems)
                    {
                        await _redisClient.SetNamedAsync(newItem);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (Setting newItem in e.NewItems)
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
        public ObservableCollection<Setting> Settings { get; }
    }
}