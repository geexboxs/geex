using Geex.Core.Testing.Api.Aggregates;
using HotChocolate.Types;

namespace Geex.Core.Testing.Api.GqlSchemas.Test.Types
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
