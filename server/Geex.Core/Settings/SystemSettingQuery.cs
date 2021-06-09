using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Geex.Common.Gql.Roots;
using Geex.Core.Settings.Domain;
using Geex.Core.Settings.GqlSchemas.Inputs;
using HotChocolate;
using HotChocolate.Types;

namespace Geex.Core.Settings
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
                    (SettingScopeEnumeration.User, async () => settingValues = await settingManager.GetUserSettingsAsync(claimsPrincipal)),
                    (SettingScopeEnumeration.Global, async () => settingValues = await settingManager.GetGlobalSettingsAsync()),
                    (SettingScopeEnumeration.Effective, async () => settingValues = await settingManager.GetAllForCurrentUserAsync(claimsPrincipal))
                );
            var result = settingValues.Join(settingDefinitions, setting => setting.Name, settingDefinition => settingDefinition.Name, (settingValue, _) => settingValue);
            return result.ToList();
        }
    }
}
