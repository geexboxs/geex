using System;
using System.Collections.Generic;
using KuanFang.Rms.MessageManagement.Messages;

namespace Geex.Common.Messaging.Api.Aggregates.Messages
{
    /// <summary>
    /// this is a aggregate root of this module, we name it the same as the module feel free to change it to its real name
    /// </summary>
    public interface IMessage
    {
        string? FromUserId { get; }
        public MessageType MessageType { get; }
        public IMessageContent Content { get; }
        IList<string> ToUserIds { get; }
        string Id { get; }
        MessageSeverityType Severity { get; set; }
    }
}
