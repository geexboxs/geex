using System.Threading.Tasks;
using Geex.Common.Gql.Roots;
using Geex.Common.Settings.Abstraction;
using Geex.Common.Settings.Core;
using HotChocolate;
using HotChocolate.Types;

namespace Geex.Common.Settings.Api
{
    [ExtendObjectType(nameof(Mutation))]
    public class SystemSettingMutation : Mutation
    {
        /// <summary>
        /// 更新设置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> UpdateSetting(
            [Service] GeexSettingManager settingManager,
            UpdateSettingInput input)
        {
            await settingManager.SetAsync(input.Name, input.Scope, input.ScopedKey, input.Value);
            return true;
        }
    }

    public class UpdateSettingInput
    {
        public SettingDefinition Name { get; set; }
        public string? Value { get; set; }
        public string? ScopedKey { get; set; }
        public SettingScopeEnumeration Scope { get; set; }
    }
}
