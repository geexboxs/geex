using System.Threading.Tasks;
using Geex.Common.Gql.Roots;
using Geex.Core.Settings.Domain;
using HotChocolate;
using HotChocolate.Types;

namespace Geex.Core.Settings
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
            [Service] ISettingManager settingManager,
            UpdateSettingInput input)
        {
            await settingManager.SetAsync(input);
            return true;
        }
    }

    public class UpdateSettingInput : IUpdateSettingParams
    {
        public SettingDefinition Name { get; set; }
        public string? Value { get; set; }
        public string? ScopedKey { get; set; }
        public SettingScopeEnumeration Scope { get; set; }
    }
}
