using System.Threading.Tasks;
using Geex.Common.Gql.Roots;
using Geex.Common.Settings.Api.Aggregates.Settings;
using Geex.Common.Settings.Api.Aggregates.Settings.Inputs;
using Geex.Common.Settings.Core;
using HotChocolate;
using HotChocolate.Types;

namespace Geex.Common.Settings.Api
{
    [ExtendObjectType(nameof(Mutation))]
    public class SettingMutation : Mutation
    {
        /// <summary>
        /// 更新设置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ISetting> UpdateSetting(
            UpdateSettingInput input)
        {
            return await this.Mediator.Send(input);
        }
    }
}
