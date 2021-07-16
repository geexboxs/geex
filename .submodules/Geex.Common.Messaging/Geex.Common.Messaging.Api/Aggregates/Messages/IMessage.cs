using System;
using System.Collections.Generic;
using Geex.Common.Abstraction;
using KuanFang.Rms.MessageManagement.Messages;

using MongoDB.Entities;

namespace Geex.Common.Messaging.Api.Aggregates.Messages
{
    /// <summary>
    /// this is a aggregate root of this module, we name it the same as the module feel free to change it to its real name
    /// </summary>
    public interface IMessage : IHasId
    {
        string? FromUserId { get; }
        public MessageType MessageType { get; }
        public IMessageContent Content { get; }
        IList<string> ToUserIds { get; }
        string Id { get; }
        MessageSeverityType Severity { get; set; }
        public string Title { get; }
        public DateTime Time { get; }
    }
}
