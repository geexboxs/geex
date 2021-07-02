using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geex.Common.Messaging.Api.Aggregates.FrontendCalls;
using HotChocolate.Types;

namespace Geex.Common.Messaging.Api.GqlSchemas.Messages.Types
{
    public class FrontendCallGqlType : ObjectType<IFrontendCall>
    {
        protected override void Configure(IObjectTypeDescriptor<IFrontendCall> descriptor)
        {
            // Implicitly binding all fields, if you want to bind fields explicitly, read more about hot chocolate
            descriptor.BindFieldsImplicitly();
            descriptor.Field(x=>x.Data).Type<AnyType>();
            base.Configure(descriptor);
        }
    }
}
