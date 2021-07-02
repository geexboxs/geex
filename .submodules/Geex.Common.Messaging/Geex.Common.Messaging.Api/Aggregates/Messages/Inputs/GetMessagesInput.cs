using System.Linq;

using MediatR;

namespace Geex.Common.Messaging.Api.Aggregates.Messages.Inputs
{
    public class GetMessagesInput : IRequest<IQueryable<IMessage>>

    {
        public MessageType? MessageType { get; set; }
    }
}
