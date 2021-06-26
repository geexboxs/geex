using System.Collections.Generic;
using Geex.Core.Testing.Api.Aggregates.TestTemplates;
using MediatR;

namespace Geex.Core.Testing.Api.GqlSchemas.TestTemplates.Inputs
{
    public class GetTestTemplatesInput:IRequest<IEnumerable<ITestTemplate>>
    {
        public string Name { get; set; }
    }
}
