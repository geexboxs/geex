using Geex.Core.Testing.Api.Aggregates.TestTemplates;
using MediatR;

namespace Geex.Core.Testing.Api.GqlSchemas.TestTemplates.Inputs
{
    public class UpdateTestTemplateInput:IRequest<ITestTemplate>
    {
        public string Name { get; set; }
        public string NewName { get; set; }
    }
}
