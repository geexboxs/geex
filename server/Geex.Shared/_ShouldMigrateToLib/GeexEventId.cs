using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace Geex.Shared._ShouldMigrateToLib
{
    public struct GeexboxEventId
    {
        public EventId val;

        public GeexboxEventId(string name)
        {
            this.val = new EventId(name.GetHashCode(), name);
        }

        public override string ToString()
        {
            return val.ToString();
        }

        public static implicit operator EventId(GeexboxEventId c)
        {
            return c.val;
        }

        public static readonly GeexboxEventId ApolloTracing = new(nameof(ApolloTracing));
    }
}
