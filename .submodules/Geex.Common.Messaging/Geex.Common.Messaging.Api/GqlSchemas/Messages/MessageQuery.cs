using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Geex.Common.Messaging.Api.Aggregates.Messages;
using Geex.Common.Messaging.Api.Aggregates.Messages.Inputs;
using Geex.Common.Gql.Roots;
using Geex.Common.Messaging.Api.GqlSchemas.Messages.Types;
using HotChocolate;
using HotChocolate.Types;
using MongoDB.Entities;

namespace Geex.Common.Messaging.Api.GqlSchemas.Messages
{
    public class MessageQuery : Query
    {
        //protected override void Configure(IObjectTypeDescriptor descriptor)
        //{
        //    //descriptor.ExtendsType<Query>();
        //    descriptor.Name(OperationTypeNames.Query);
        //    descriptor
        //        .Field("messages")
        //        .Type<ListType<MessageGqlType>>()
        //        .Resolve(async context =>
        //        {
        //            var input = context.ArgumentValue<GetMessagesInput>("input");
        //            var result = await Mediator.Send(input);
        //            return result;
        //        })
        //    //.UsePaging()
        //    ;
        //}
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
