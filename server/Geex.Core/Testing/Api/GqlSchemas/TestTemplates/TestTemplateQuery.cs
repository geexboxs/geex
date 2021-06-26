using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Geex.Common.Gql.Roots;
using Geex.Core.Testing.Api.Aggregates.TestTemplates;
using Geex.Core.Testing.Api.GqlSchemas.TestTemplates.Inputs;
using HotChocolate.Types;

namespace Geex.Core.Testing.Api.GqlSchemas.TestTemplates
{
    [ExtendObjectType(nameof(Query))]
    public class TestTemplateQuery : Query
    {
        /// <summary>
        /// 根据provider获取全量设置
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<List<ITestTemplate>> TestTemplates(
            GetTestTemplatesInput input)
        {
            var result = await Mediator.Send(input);
            return result.ToList();
        }
    }
}
