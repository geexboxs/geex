using Geex.Common.Messaging.Api.Aggregates.Messages;

using HotChocolate.Types;

namespace Geex.Common.Messaging.Api.GqlSchemas.Messages.Types
{
    public class MessageGqlType : ObjectType<IMessage>
    {
        protected override void Configure(IObjectTypeDescriptor<IMessage> descriptor)
        {
            descriptor.BindFieldsImplicitly();
            base.Configure(descriptor);
        }
    }
}
