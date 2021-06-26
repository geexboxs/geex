using Geex.Core.Testing.Api.Aggregates.Tests;
using MediatR;

namespace Geex.Core.Testing.Api.GqlSchemas.Tests.Inputs
{
    public class UpdateTestInput:IRequest<ITest>
    {
        public string Name { get; set; }
        public string NewName { get; set; }
    }
}
