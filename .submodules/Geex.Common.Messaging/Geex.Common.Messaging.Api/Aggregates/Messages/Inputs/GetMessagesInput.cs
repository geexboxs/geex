using System.Collections.Generic;
using System.Linq;

using HotChocolate.Data.Filters;

using MediatR;

namespace Geex.Common.Messaging.Api.Aggregates.Messages.Inputs
{
    public class GetMessagesInput : FilterInputType<IMessage>, IRequest<IQueryable<IMessage>>
    {
        protected override void Configure(
        IFilterInputTypeDescriptor<IMessage> descriptor)
        {
            descriptor.BindFieldsImplicitly();
        }

        public MessageType? MessageType { get; set; }
    }
}
