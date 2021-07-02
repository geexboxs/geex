using System;
using KuanFang.Rms.MessageManagement.Messages;

namespace Geex.Common.Messaging.Core.Aggregates.Messages
{
    public class InteractContent:IMessageContent
    {
        public string Title { get; }
        public DateTime Time { get; }
    }
}
