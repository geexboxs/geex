using System.Security;
using System.Security.Claims;
using System.Threading.Tasks;

using Geex.Common.Gql.Roots;
using Geex.Common.Messaging.Api.Aggregates.FrontendCalls;
using Geex.Common.Messaging.Api.Aggregates.Messages;
using Geex.Common.Messaging.Core.Aggregates.FrontendCalls;

using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Execution;
using HotChocolate.Subscriptions;
using HotChocolate.Types;

namespace Geex.Common.Messaging.Api.GqlSchemas.Messages
{
    [ExtendObjectType(nameof(Subscription))]
    public class MessageSubscription : Subscription
    {
        public ValueTask<ISourceStream<IFrontendCall>> SubscribeToFrontendCall(
        [Service] ITopicEventReceiver receiver,
            [Service] ClaimsPrincipal claimsPrincipal)
        => receiver.SubscribeAsync<string, IFrontendCall>(claimsPrincipal.FindUserId());

        [Subscribe(With = nameof(SubscribeToFrontendCall))]
        public IFrontendCall OnFrontendCall([EventMessage] IFrontendCall frontendCall)
            => frontendCall;
    }
}
