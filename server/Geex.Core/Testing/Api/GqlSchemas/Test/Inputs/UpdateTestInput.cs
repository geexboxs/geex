using Geex.Core.Testing.Api.Aggregates;
using MediatR;

namespace Geex.Core.Testing.Api.GqlSchemas.Test.Inputs
{
    public class UpdateTestInput:IRequest<ITest>
    {
        public string Name { get; set; }
        public string NewName { get; set; }
    }
}
