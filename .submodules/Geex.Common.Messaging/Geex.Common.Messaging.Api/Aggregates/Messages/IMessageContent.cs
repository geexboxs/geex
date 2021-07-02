using System;

namespace KuanFang.Rms.MessageManagement.Messages
{
    public interface IMessageContent
    {
        public string Title { get; }
        public DateTime Time { get; }
    }
}