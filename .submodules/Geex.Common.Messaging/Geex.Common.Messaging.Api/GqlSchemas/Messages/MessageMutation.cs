using System.Threading.Tasks;
using Geex.Common.Messaging.Api.Aggregates.Messages;
using Geex.Common.Messaging.Api.Aggregates.Messages.Inputs;
using Geex.Common.Gql.Roots;
using HotChocolate;
using HotChocolate.Subscriptions;
using HotChocolate.Types;
using MongoDB.Entities;

namespace Geex.Common.Messaging.Api.GqlSchemas.Messages
{
    [ExtendObjectType(nameof(Mutation))]
    public class MessageMutation : Mutation
    {
        /// <summary>
        /// 标记消息已读
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> MarkMessagesRead(
            MarkMessagesReadInput input)
        {
            var result = await Mediator.Send(input);
            return true;
        }
        /// <summary>
        /// 标记消息已读
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> DeleteMessageDistributions(
            DeleteMessageDistributionsInput input)
        {
            var result = await Mediator.Send(input);
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> SendMessage(SendNotificationMessageRequest input)
        {
            var result = await Mediator.Send(input);
            return true;
        }

    }
}