using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Geex.Core.SystemSettings.Domain;
using HotChocolate;
using Volo.Abp.Settings;

namespace Geex.Core.SystemSettings
{
    public class SystemSettingMutation
    {
        /// <summary>
        /// 更新设置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateSetting(
            [Service] ISettingManager settingManager,
            UpdateSettingInput input)
        {
            await settingManager.SetAsync(input);
        }
    }

    public interface ISettingManager
    {
        public async Task SetAsync(ISettingValue setting)
        {
            this.Definitions
        }

        SettingDefinition Definitions { get; }
    }

    public class UpdateSettingInput:ISettingValue
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string ProviderKey { get; set; }
        public SettableSettingProviderEnumeration SettingProvider { get; set; }
    }
}
