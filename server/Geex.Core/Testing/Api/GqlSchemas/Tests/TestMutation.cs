using System.Threading.Tasks;
using Geex.Common.Gql.Roots;
using Geex.Core.Testing.Api.Aggregates.Tests;
using Geex.Core.Testing.Api.GqlSchemas.Tests.Inputs;
using HotChocolate;
using HotChocolate.Types;
using MediatR;

namespace Geex.Core.Testing.Api.GqlSchemas.Tests
{
    public class TestMutation : MutationTypeExtension<TestMutation>
    {
        /// <summary>
        /// 更新设置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ITest> UpdateTest(
            [Service] IMediator Mediator,
            UpdateTestInput input)
        {
            var result = await Mediator.Send(input);
            return result;
        }
    }
}
