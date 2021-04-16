using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Geex.Core.SystemSettings.Domain;
using Geex.Shared._ShouldMigrateToLib;
using Geex.Shared.Roots;

using HotChocolate;
using HotChocolate.Types;

using MongoDB.Entities;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;

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
        public async Task<List<SettingValue>> Settings(
            [Service] ISettingDefinitionManager settingDefinitionManager,
            [Service] ISettingManager settingManager,
            GetSettingsInput dto)
        {
            var settingDefinitions = settingDefinitionManager.GetAll().Where(x => !x.Providers.Any() || x.Providers.Contains(dto.Provider));
            List<SettingValue> settingValues = new List<SettingValue>();
            await dto.Provider.SwitchAsync(
                 (SettableSettingProviderEnumeration.User, async () => settingValues = await settingManager.GetAllForCurrentUserAsync()),
                 (SettableSettingProviderEnumeration.Tenant, async () => settingValues = await settingManager.GetAllForCurrentTenantAsync()),
                 (SettableSettingProviderEnumeration.Global, async () => settingValues = await settingManager.GetAllGlobalAsync())
             );
            var result = settingValues.Join(settingDefinitions, settingValue => settingValue.Name, settingDefinition => settingDefinition.Name, (settingValue, _) => settingValue);
            return result.ToList();
        }
    }

    public class GetSettingsInput
    {
        public SettableSettingProviderEnumeration Provider { get; set; }
    }
}
