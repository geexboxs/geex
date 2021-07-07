using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using Geex.Common.Abstractions;
using Geex.Common.Abstractions.Enumerations;
using Geex.Common.Settings.Abstraction;
using Geex.Common.Settings.Api;
using Geex.Common.Settings.Api.Aggregates.Settings;
using Geex.Common.Settings.Api.Aggregates.Settings.Inputs;

using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using MongoDB.Entities;

using MoreLinq;

using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Core.Abstractions;

using Volo.Abp.DependencyInjection;

namespace Geex.Common.Settings.Core
{
    public class SettingHandler : IRequestHandler<UpdateSettingInput, ISetting>, IRequestHandler<GetSettingsInput, IQueryable<ISetting>>
    {
        public ILogger<SettingHandler> Logger { get; }
        private IRedisDatabase _redisClient;
        private readonly DbContext _dbContext;
        private readonly ClaimsPrincipal _principal;
        private static IReadOnlyList<SettingDefinition> _settingDefinitions;


        public SettingHandler(IRedisDatabase redisClient, IEnumerable<GeexModule> modules, DbContext dbContext, ClaimsPrincipal principal, ILogger<SettingHandler> logger)
        {
            Logger = logger;
            _redisClient = redisClient;
            _dbContext = dbContext;
            _principal = principal;
            if (_settingDefinitions == default)
            {
                var definitionTypes = modules
                   .Select(y => y.GetType().Assembly).Distinct()
                   .SelectMany(y => y.DefinedTypes
                   .Where(z => z.BaseType == typeof(SettingDefinition)));
                var settingDefinitions =
                    definitionTypes.SelectMany(x => x.GetProperties(BindingFlags.Static | BindingFlags.Public).Where(y => y.DeclaringType.IsAssignableTo(x))).Select(x => x.GetValue(null)).Cast<SettingDefinition>();
                _settingDefinitions = new ReadOnlyCollection<SettingDefinition>(settingDefinitions.ToArray());
            }
        }

        public IReadOnlyList<SettingDefinition> SettingDefinitions => _settingDefinitions;

        public async Task<Setting> SetAsync(SettingDefinition settingDefinition, SettingScopeEnumeration scope, string? scopedKey, string? value)
        {
            var definition = this.SettingDefinitions.FirstOrDefault(x => x.Name == settingDefinition);
            if (definition == default)
            {
                throw new BusinessException(GeexExceptionType.NotFound, message: "setting name not exists.");
            }
            var setting = await _dbContext.Find<Setting>().Match(x => x.Name == settingDefinition && x.Scope == scope && x.ScopedKey == scopedKey).ExecuteSingleAsync();
            setting.SetValue(value);
            await setting.SaveAsync();
            _dbContext.OnCommitted += async (sender) =>
             {
                 await _redisClient.SetNamedAsync(setting.GetRedisKey());
             };
            return setting;
        }

        public async Task<IEnumerable<Setting>> GetAllForCurrentUserAsync(ClaimsPrincipal identity)
        {
            var globalSettings = await this.GetGlobalSettingsAsync();
            if (!identity.FindUserId().IsNullOrEmpty())
            {
                var userSettings = await this.GetUserSettingsAsync(identity);
                return userSettings.Union(globalSettings, new GenericEqualityComparer<Setting>().With(x => x.Name));
            }
            return globalSettings;
        }
        public async Task<IEnumerable<Setting>> GetGlobalSettingsAsync()
        {
            var globalSettings = await _redisClient.GetAllNamedByKeyAsync<Setting>($"{SettingScopeEnumeration.Global}:*");
            if (SettingDefinitions.Except(globalSettings.Select(x => x.Value.Name)).Any())
            {
                var dbSettings = await _dbContext.Find<Setting>().Match(x => x.Scope == SettingScopeEnumeration.Global).ExecuteAsync();
                await TrySyncSettings(globalSettings, dbSettings);
                return dbSettings;
            }
            return globalSettings.Values;
        }

        private async Task TrySyncSettings(IDictionary<string, Setting> cachedSettings, List<Setting> dbSettings)
        {
            if (cachedSettings.Values.OrderBy(x => x.Name).SequenceEqual(dbSettings.OrderBy(x => x.Name), new GenericEqualityComparer<Setting>().With(x => x.Name)))
            {
                return;
            }
            await _redisClient.RemoveAllAsync(cachedSettings.Keys);
            _ = await _redisClient.AddAllAsync(dbSettings.Select(x => new Tuple<string, Setting>(x.GetRedisKey(), x)).ToList());
        }

        public async Task<IEnumerable<Setting>> GetUserSettingsAsync(ClaimsPrincipal identity)
        {
            var userSettings = await _redisClient.GetAllNamedByKeyAsync<Setting>($"{SettingScopeEnumeration.User}:{identity.FindUserId()}:*");
            if (SettingDefinitions.Except(userSettings.Select(x => x.Value.Name)).Any())
            {
                var dbSettings = await _dbContext.Find<Setting>().Match(x => x.Scope == SettingScopeEnumeration.User && x.ScopedKey == identity.FindUserId()).ExecuteAsync();
                await TrySyncSettings(userSettings, dbSettings);
                return dbSettings;
            }
            return userSettings.Values;
        }

        public async Task<Setting?> GetOrNullAsync(SettingDefinition settingDefinition, SettingScopeEnumeration settingScope = default,
            string? scopedKey = default)
        {
            return await _redisClient.GetNamedAsync<Setting>(new Setting(settingDefinition, default, settingScope, scopedKey).GetRedisKey());
        }

        public async Task<ISetting> Handle(UpdateSettingInput request, CancellationToken cancellationToken)
        {
            return await SetAsync(request.Name, request.Scope, request.ScopedKey, request.Value);
        }

        public async Task<IQueryable<ISetting>> Handle(GetSettingsInput request, CancellationToken cancellationToken)
        {
            var settingDefinitions = this.SettingDefinitions;
            IEnumerable<Setting> settingValues = Enumerable.Empty<Setting>();
            if (request.Scope != default)
            {
                await request.Scope.SwitchAsync(
                    (SettingScopeEnumeration.User, async () => settingValues = await this.GetUserSettingsAsync(_principal)),
                    (SettingScopeEnumeration.Global, async () => settingValues = await this.GetGlobalSettingsAsync())
                );
            }
            else
            {
                settingValues = await this.GetAllForCurrentUserAsync(_principal);
            }
            var result = settingValues/*.Join(settingDefinitions, setting => setting.Name, settingDefinition => settingDefinition.Name, (settingValue, _) => settingValue)*/;
            return result.AsQueryable();
        }
    }
}