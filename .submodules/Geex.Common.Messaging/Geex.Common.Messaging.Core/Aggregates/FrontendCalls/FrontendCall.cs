using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Geex.Common.Messaging.Api.Aggregates.FrontendCalls;

namespace Geex.Common.Messaging.Core.Aggregates.FrontendCalls
{
    public class FrontendCall : IFrontendCall
    {
        public FrontendCall(FrontendCallType frontendCallType, object data)
        {
            FrontendCallType = frontendCallType;
            Data = data;
        }

        public FrontendCallType FrontendCallType { get; }
        public object Data { get; }
    }

}
