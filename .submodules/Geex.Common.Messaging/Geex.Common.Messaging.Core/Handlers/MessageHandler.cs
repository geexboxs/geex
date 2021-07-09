using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Geex.Common.Abstraction.Gql.Inputs;
using Geex.Common.Abstractions;
using Geex.Common.Messaging.Api.Aggregates.FrontendCalls;
using Geex.Common.Messaging.Api.Aggregates.Messages;
using Geex.Common.Messaging.Api.Aggregates.Messages.Inputs;
using Geex.Common.Messaging.Api.GqlSchemas.Messages;
using Geex.Common.Messaging.Core.Aggregates;
using Geex.Common.Messaging.Core.Aggregates.FrontendCalls;
using Geex.Common.Messaging.Core.Aggregates.Messages;

using HotChocolate.Subscriptions;

using MediatR;

using MongoDB.Entities;

namespace Geex.Common.Messaging.Core.Handlers
{
    public class MessageHandler :
        IRequestHandler<QueryInput<IMessage>, IQueryable<IMessage>>,
        IRequestHandler<DeleteMessageDistributionsInput, Unit>,
        IRequestHandler<MarkMessagesReadInput, Unit>,
        IRequestHandler<SendNotificationMessageRequest, Unit>,
        IRequestHandler<GetUnreadMessagesInput, IEnumerable<IMessage>>
    {
        public DbContext DbContext { get; }
        public ClaimsPrincipal ClaimsPrincipal { get; }
        public LazyFactory<ITopicEventSender> Sender { get; }

        public MessageHandler(DbContext dbContext, ClaimsPrincipal claimsPrincipal, LazyFactory<ITopicEventSender> sender)
        {
            DbContext = dbContext;
            ClaimsPrincipal = claimsPrincipal;
            Sender = sender;
        }

        public async Task<IQueryable<IMessage>> Handle(QueryInput<IMessage> input,
            CancellationToken cancellationToken)
        {
            return DbContext.Queryable<Message>();
        }

        public async Task<Unit> Handle(DeleteMessageDistributionsInput request, CancellationToken cancellationToken)
        {
            var res = await DbContext.DeleteAsync<MessageDistribution>(x => request.UserIds.Contains(x.ToUserId) && request.MessageId == x.MessageId, cancellationToken);
            return Unit.Value;
        }

        public async Task<Unit> Handle(MarkMessagesReadInput request, CancellationToken cancellationToken)
        {
            await DbContext.Update<MessageDistribution>().Match(x => request.MessageIds.Contains(x.MessageId) && request.UserId == x.ToUserId).Modify(x => x.IsRead, true).ExecuteAsync(cancellationToken);
            return Unit.Value;
        }

        public async Task<IEnumerable<IMessage>> Handle(GetUnreadMessagesInput request, CancellationToken cancellationToken)
        {
            var messageDistributions = await DbContext.Find<MessageDistribution>().Match(x => x.IsRead == false && x.ToUserId == ClaimsPrincipal.FindUserId()).ExecuteAsync(cancellationToken);
            var messageIds = messageDistributions.Select(x => x.MessageId);
            var messages = await DbContext.Find<Message>().ManyAsync(x => messageIds.Contains(x.Id), cancellationToken);
            return messages;
        }

        public async Task<Unit> Handle(SendNotificationMessageRequest request, CancellationToken cancellationToken)
        {
            var message = new Message(request.Text, request.Severity);
            DbContext.AttachContextSession(message);
            await message.SaveAsync(cancellation: cancellationToken);
            await message.DistributeAsync(request.ToUserIds.ToArray());

            await Sender.Value.SendAsync(ClaimsPrincipal.FindUserId(), new FrontendCall(FrontendCallType.NewMessage, new { message.Content, message.FromUserId, message.MessageType, message.Severity }) as IFrontendCall
           , cancellationToken);
            return Unit.Value;
        }
    }
}
