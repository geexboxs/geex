using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Geex.Common.Abstractions;
using Geex.Common.Gql.Types;
using HotChocolate;

namespace Geex.Common.Messaging.Api.Aggregates.FrontendCalls
{
    public interface IFrontendCall
    {
        public FrontendCallType FrontendCallType { get; }
        public object Data { get; }
    }
    public class FrontendCallType : Enumeration<FrontendCallType, string>
    {
        protected FrontendCallType(string name, string value) : base(name, value)
        {
        }

        public static FrontendCallType NewMessage { get; } = new(nameof(NewMessage), nameof(NewMessage));
    }

}
