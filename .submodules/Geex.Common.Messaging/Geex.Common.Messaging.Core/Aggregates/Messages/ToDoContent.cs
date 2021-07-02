using System;
using KuanFang.Rms.MessageManagement.Messages;

namespace Geex.Common.Messaging.Core.Aggregates.Messages
{
    public class ToDoContent : IMessageContent
    {
        public ToDoContent(string title)
        {
            this.Title = title;
        }

        public ToDoContent(string title, TodoType todoType, object meta) : this(title)
        {
            this.TodoType = todoType;
            this.Meta = meta;
        }

        public string Title { get; }
        public string Detail { get; set; }
        public DateTime Time { get; } = DateTime.Now;
        public TodoType TodoType { get; set; }
        public object Meta { get; }
    }
}
