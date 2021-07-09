using System.Threading.Tasks;
using Geex.Common.Gql.Roots;
using Geex.Core.Testing.Api.Aggregates.TestTemplates;
using Geex.Core.Testing.Api.GqlSchemas.TestTemplates.Inputs;
using HotChocolate;
using HotChocolate.Types;
using MediatR;

namespace Geex.Core.Testing.Api.GqlSchemas.TestTemplates
{
    [ExtendObjectType(nameof(Mutation))]
    public class TestTemplateMutation : Mutation
    {
        /// <summary>
        /// 更新设置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ITestTemplate> UpdateTestTemplate(
            [Service] IMediator Mediator,
            UpdateTestTemplateInput input)
        {
            var result = await Mediator.Send(input);
            return result;
        }
    }
}
