using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Geex.Common.Gql.Roots;

using HotChocolate.Types;

namespace Geex.Core.Testing.Api.GqlSchemas.Test
{
    [ExtendObjectType(nameof(Subscription))]
    public class TestSubscription : Subscription
    {
        // todo
    }
}
