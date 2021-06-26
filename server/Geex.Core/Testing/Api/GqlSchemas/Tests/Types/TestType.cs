using Geex.Core.Testing.Api.Aggregates.Tests;
using HotChocolate.Types;

namespace Geex.Core.Testing.Api.GqlSchemas.Tests.Types
{
    public class TestType:ObjectType<ITest>
    {
        protected override void Configure(IObjectTypeDescriptor<ITest> descriptor)
        {
            descriptor.BindFieldsImplicitly();
            base.Configure(descriptor);
        }
    }
}
