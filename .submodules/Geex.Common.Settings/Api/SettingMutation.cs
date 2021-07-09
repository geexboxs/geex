using System.Threading.Tasks;

using Geex.Common.Gql.Roots;
using Geex.Common.Settings.Api.Aggregates.Settings;
using Geex.Common.Settings.Api.Aggregates.Settings.Inputs;
using Geex.Common.Settings.Core;

using HotChocolate;
using HotChocolate.Types;

using MediatR;

namespace Geex.Common.Settings.Api
{
    public class SettingMutation : MutationTypeExtension<SettingMutation>
    {
        /// <summary>
        /// 更新设置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ISetting> UpdateSetting(
            [Service] IMediator Mediator,
            UpdateSettingInput input)
        {
            return await Mediator.Send(input);
        }
    }
}
