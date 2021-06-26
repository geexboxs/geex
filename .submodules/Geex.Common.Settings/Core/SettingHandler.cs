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
using MongoDB.Entities;
using MoreLinq;

using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Geex.Common.Settings.Core
{
    public class SettingHandler : IRequestHandler<UpdateSettingInput, ISetting>, IRequestHandler<GetSettingsInput, IQueryable<ISetting>>
    {
        private IRedisDatabase _redisClient;
        private readonly DbContext _dbContext;
        private readonly Lazy<ClaimsPrincipal> _principalLazyInject;

        public SettingHandler(IRedisDatabase redisClient, IEnumerable<GeexModule> modules, DbContext dbContext, Lazy<ClaimsPrincipal> principalLazyInject)
        {
            _redisClient = redisClient;
            _dbContext = dbContext;
            _principalLazyInject = principalLazyInject;
            var definitionTypes = modules
                    .Select(y => y.GetType().Assembly).Distinct()
                    .SelectMany(y => y.DefinedTypes
                    .Where(z => z.BaseType == typeof(SettingDefinition)));
            var settingDefinitions =
                definitionTypes.SelectMany(x => x.GetProperties(BindingFlags.Static | BindingFlags.Public).Where(y => y.DeclaringType.IsAssignableTo(x))).Select(x => x.GetValue(null)).Cast<SettingDefinition>();
            SettingDefinitions = new ReadOnlyCollection<SettingDefinition>(settingDefinitions.ToArray());
            //var defaultSettings = SettingDefinitions.Select(x => new Setting(x, x.DefaultValue, SettingScopeEnumeration.Global));
            //var existedSettings = _redisClient.GetAllNamedAsync<Setting>().Result.Select(x => x.Value);
            //var userSettings = existedSettings.Where(x => x.Scope == SettingScopeEnumeration.User);
            //var overrideSettings = existedSettings.Where(x => x.Scope == SettingScopeEnumeration.Global);
            //var overrideSettingNames = overrideSettings.Select(x => x.Name);

            //var overridedSettings = overrideSettings.Union(defaultSettings, new GenericEqualityComparer<Setting>().With(x=>x.Name));
        }

        public IReadOnlyList<SettingDefinition> SettingDefinitions { get; }

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
            var setting = new Setting(definition, value, scope, scopedKey);
            if (await _redisClient.SetNamedAsync(setting.RedisKey))
            {
                return setting;
            }
            throw new BusinessException(GeexExceptionType.Unknown, message: "failed to set setting");
        }

        public async Task<IEnumerable<Setting>> GetAllForCurrentUserAsync(ClaimsPrincipal identity)
        {
            var userKeys = await _redisClient.SearchKeysAsync($"{nameof(Setting)}:{SettingScopeEnumeration.User}:{identity.FindUserId()}:*");
            var userSettings = await _redisClient.GetAllAsync<Setting>(userKeys);
            var globalKeys = await _redisClient.SearchKeysAsync($"{nameof(Setting)}:{SettingScopeEnumeration.Global}:*");
            var globalSettings = await _redisClient.GetAllAsync<Setting>(globalKeys);
            return userSettings.Values.Union(globalSettings.Values, new GenericEqualityComparer<Setting>().With(x => x.Name));
        }
        public async Task<IEnumerable<Setting>> GetGlobalSettingsAsync()
        {
            var globalKeys = await _redisClient.SearchKeysAsync($"{nameof(Setting)}:{SettingScopeEnumeration.Global}:*");
            var globalSettings = await _redisClient.GetAllAsync<Setting>(globalKeys);
            return globalSettings.Values;
        }

        public async Task<IEnumerable<Setting>> GetUserSettingsAsync(ClaimsPrincipal identity)
        {
            var userKeys = await _redisClient.SearchKeysAsync($"{nameof(Setting)}:{SettingScopeEnumeration.User}:{identity.FindUserId()}:*");
            var userSettings = await _redisClient.GetAllAsync<Setting>(userKeys);
            return userSettings.Values;
        }

        public async Task<Setting?> GetOrNullAsync(SettingDefinition settingDefinition, SettingScopeEnumeration settingScope = default,
            string? scopedKey = default)
        {
            return await _redisClient.GetNamedAsync<Setting>(new Setting(settingDefinition, default, settingScope, scopedKey).RedisKey);
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
                    (SettingScopeEnumeration.User, async () => settingValues = await this.GetUserSettingsAsync(_principalLazyInject.Value)),
                    (SettingScopeEnumeration.Global, async () => settingValues = await this.GetGlobalSettingsAsync())
                );
            }
            else
            {
                settingValues = await this.GetAllForCurrentUserAsync(_principalLazyInject.Value);
            }
            var result = settingValues/*.Join(settingDefinitions, setting => setting.Name, settingDefinition => settingDefinition.Name, (settingValue, _) => settingValue)*/;
            return result.AsQueryable();
        }
    }
}