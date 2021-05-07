using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Geex.Core.SystemSettings.Domain;
using Geex.Shared._ShouldMigrateToLib;
using Geex.Shared._ShouldMigrateToLib.Abstractions;
using Geex.Shared.Roots;

using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;

using MongoDB.Entities;

namespace Geex.Core.SystemSettings
{
    [ExtendObjectType(nameof(Query))]
    public class SystemSettingQuery : Query
    {
        /// <summary>
        /// 根据provider获取全量设置
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<List<Setting>> Settings(
            [Service] ISettingManager settingManager,
            [Service] ClaimsPrincipal claimsPrincipal,
            GetSettingsInput dto)
        {
            var settingDefinitions = settingManager.SettingDefinitions;
            IEnumerable<Setting> settingValues = Enumerable.Empty<Setting>();
            await dto.Scope.SwitchAsync(
                 (SettingScopeEnumeration.User, async () => settingValues = await settingManager.GetAllForUserAsync(claimsPrincipal)),
                 (SettingScopeEnumeration.Global, async () => settingValues = await settingManager.GetAllGlobalAsync())
             );
            var result = settingValues.Join(settingDefinitions, setting => setting.Name, settingDefinition => settingDefinition.Name, (settingValue, _) => settingValue);
            return result.ToList();
        }
    }
}
