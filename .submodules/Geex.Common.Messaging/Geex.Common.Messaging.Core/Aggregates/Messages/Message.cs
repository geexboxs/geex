using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonServiceLocator;
using Geex.Common.Abstractions;
using Geex.Common.Messaging.Api.Aggregates.Messages;
using KuanFang.Rms.MessageManagement.Messages;
using Microsoft.Extensions.Logging;

namespace Geex.Common.Messaging.Core.Aggregates.Messages
{
    /// <summary>
    /// this is a aggregate root of this module, we name it the same as the module feel free to change it to its real name
    /// </summary>
    public partial class Message : Entity, IMessage
    {
        private ILogger<Message> Logger => ServiceLocator.Current.GetInstance<ILogger<Message>>();
        protected Message()
        {

        }
        public Message(IMessageContent content, MessageSeverityType severity = MessageSeverityType.Info)
        {
            this.Content = content;
            this.Severity = severity;
            this.MessageType = content switch
            {
                NotificationContent => MessageType.Notification,
                ToDoContent => MessageType.Todo,
                InteractContent => MessageType.Interact,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public virtual ICollection<MessageDistribution> Distributions { get; private set; } = new List<MessageDistribution>();
        public string? FromUserId { get; private set; }
        public MessageType MessageType { get; private set; }
        public IMessageContent Content { get; private set; }
        public MessageSeverityType Severity { get; set; }
        public async Task<Message> DistributeAsync(params string[] userIds)
        {
            if (this.Id == string.Empty)
            {
                throw new NotSupportedException("必须先保存消息才能进行消息分配");
            }

            if (this.Distributions.Any())
            {
                throw new NotSupportedException("消息已经被分配");
            }

            foreach (var userId in userIds)
            {
                this.Distributions.Add(new MessageDistribution(this.Id, userId));
            }

            return this;
        }

        /// <summary>
        /// 标记当前消息针对特定用户已读
        /// </summary>
        /// <param name="userId"></param>
        public void MarkAsRead(string userId)
        {
            var userDistribution = this.Distributions.FirstOrDefault(x => x.ToUserId == userId);
            if (userDistribution != default)
            {
                userDistribution.IsRead = true;
            }
            else
            {
                Logger.LogWarning("试图标记不存在的消息分配记录已读.");
            }
        }
    }

    public partial class Message
    {
        public IList<string> ToUserIds => this.Distributions.ToList().Select(x => x.ToUserId).ToList();
    }
}
