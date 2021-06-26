using System.Collections.Generic;
using Geex.Core.Testing.Api.Aggregates.Tests;
using MediatR;

namespace Geex.Core.Testing.Api.GqlSchemas.Tests.Inputs
{
    public class GetTestsInput:IRequest<IEnumerable<ITest>>
    {
        public string Name { get; set; }
    }
}
