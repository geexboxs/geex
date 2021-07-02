using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Geex.Common.Messaging.Api.Aggregates.Messages;
using Geex.Common.Messaging.Api.Aggregates.Messages.Inputs;
using Geex.Common.Gql.Roots;
using HotChocolate;
using HotChocolate.Types;
using MongoDB.Entities;

namespace Geex.Common.Messaging.Api.GqlSchemas.Messages
{
    [ExtendObjectType(nameof(Query))]
    public class MessageQuery : Query
    {
        /// <summary>
        /// 列表获取message
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IQueryable<IMessage>> Messages(
            GetMessagesInput input)
        {
            var result = await Mediator.Send(input);
            return result;
        }
        /// <summary>
        /// 列表获取message
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IQueryable<IMessage>> UnreadMessages(
            GetUnreadMessagesInput input)
        {
            var result = await Mediator.Send(input);
            return result;
        }
    }
}
