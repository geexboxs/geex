using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Geex.Common.Gql.Roots;
using Geex.Common.Settings.Abstraction;
using Geex.Common.Settings.Api.Aggregates.Settings;
using Geex.Common.Settings.Api.Aggregates.Settings.Inputs;
using Geex.Common.Settings.Core;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;

using MediatR;

namespace Geex.Common.Settings.Api
{
    public class SettingQuery : QueryTypeExtension<SettingQuery>
    {
        /// <summary>
        /// 根据provider获取全量设置
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IQueryable<ISetting>> Settings(
            [Service] IMediator Mediator,
            GetSettingsInput input)
        {
            return await Mediator.Send(input);
        }
    }
}
