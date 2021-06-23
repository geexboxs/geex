using System.Collections.Generic;
using Geex.Core.Testing.Api.Aggregates;
using MediatR;

namespace Geex.Core.Testing.Api.GqlSchemas.Test.Inputs
{
    public class GetTestsInput:IRequest<IEnumerable<ITest>>
    {
        public string Name { get; set; }
    }
}
