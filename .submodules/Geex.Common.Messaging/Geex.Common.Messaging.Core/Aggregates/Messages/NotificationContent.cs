using System;
using KuanFang.Rms.MessageManagement.Messages;

namespace Geex.Common.Messaging.Core.Aggregates.Messages
{
    public class NotificationContent : IMessageContent
    {
        public NotificationContent(string title)
        {
            this.Title = title;
            Time = DateTime.Now;
        }

        public string Title { get; }
        public DateTime Time { get; }
    }


}
