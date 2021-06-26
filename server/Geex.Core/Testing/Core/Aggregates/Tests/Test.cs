using Geex.Common.Abstractions;
using Geex.Core.Testing.Api.Aggregates.Tests;

namespace Geex.Core.Testing.Core.Aggregates.Tests
{
    public class Test : Entity, ITest
    {
        public string Name { get; set; }
        public string Data { get; set; }
    }
}
