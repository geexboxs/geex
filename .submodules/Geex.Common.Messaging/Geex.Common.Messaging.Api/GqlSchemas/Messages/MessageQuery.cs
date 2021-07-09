using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Geex.Common.Abstraction.Gql.Inputs;
using Geex.Common.Messaging.Api.Aggregates.Messages;
using Geex.Common.Messaging.Api.Aggregates.Messages.Inputs;
using Geex.Common.Gql.Roots;
using Geex.Common.Messaging.Api.GqlSchemas.Messages.Types;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Pagination;
using MediatR;
using MongoDB.Entities;

namespace Geex.Common.Messaging.Api.GqlSchemas.Messages
{
    public class MessageQuery : QueryTypeExtension<MessageQuery>
    {
        protected override void Configure(IObjectTypeDescriptor<MessageQuery> descriptor)
        {
            descriptor.ResolveMethod(x => x.Messages(default))
            .UseOffsetPaging<MessageGqlType>()
            .UseFiltering<IMessage>(x => x.Field(y => y.MessageType))
            ;
            base.Configure(descriptor);
        }

        /// <summary>
        /// 列表获取message
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IQueryable<IMessage>> Messages(
            [Service] IMediator Mediator)
        {
            var result = await Mediator.Send(new QueryInput<IMessage>());
            return result;
        }

        /// <summary>
        /// 列表获取message
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IQueryable<IMessage>> UnreadMessages(
            [Service] IMediator Mediator,
            GetUnreadMessagesInput input)
        {
            var result = await Mediator.Send(input);
            return result;
        }
    }
}
