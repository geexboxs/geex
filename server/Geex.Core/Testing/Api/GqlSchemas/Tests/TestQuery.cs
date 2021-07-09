using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Geex.Common.Gql.Roots;
using Geex.Core.Testing.Api.Aggregates.Tests;
using Geex.Core.Testing.Api.GqlSchemas.Tests.Inputs;

using HotChocolate;
using HotChocolate.Types;

using MediatR;

namespace Geex.Core.Testing.Api.GqlSchemas.Tests
{
    public class TestQuery : QueryTypeExtension<TestQuery>
    {
        /// <summary>
        /// 根据provider获取全量设置
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<List<ITest>> Tests(
            [Service] IMediator Mediator,
            GetTestsInput input)
        {
            var result = await Mediator.Send(input);
            return result.ToList();
        }
    }
}
