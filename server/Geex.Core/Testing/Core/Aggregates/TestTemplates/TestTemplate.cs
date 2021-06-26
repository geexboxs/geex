using Geex.Common.Abstractions;
using Geex.Core.Testing.Api.Aggregates.TestTemplates;

namespace Geex.Core.Testing.Core.Aggregates.TestTemplates
{
    public class TestTemplate : Entity, ITestTemplate
    {
        public string Name { get; set; }
    }
}
