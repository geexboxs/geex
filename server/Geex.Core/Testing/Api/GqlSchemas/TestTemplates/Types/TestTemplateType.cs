using Geex.Core.Testing.Api.Aggregates.TestTemplates;
using HotChocolate.Types;

namespace Geex.Core.Testing.Api.GqlSchemas.TestTemplates.Types
{
    public class TestTemplateType:ObjectType<ITestTemplate>
    {
        protected override void Configure(IObjectTypeDescriptor<ITestTemplate> descriptor)
        {
            descriptor.BindFieldsImplicitly();
            base.Configure(descriptor);
        }
    }
}