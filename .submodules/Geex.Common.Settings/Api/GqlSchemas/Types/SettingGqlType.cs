using Geex.Common.Settings.Api.Aggregates.Settings;
using HotChocolate.Types;

namespace Geex.Common.Settings.Api.GqlSchemas.Types
{
    public class SettingGqlType : ObjectType<ISetting>
    {
        protected override void Configure(IObjectTypeDescriptor<ISetting> descriptor)
        {
            descriptor.BindFieldsImplicitly();
            descriptor.Field(x => x.Scope);
            base.Configure(descriptor);
        }
    }
}
