using System;
using System.Threading.Tasks;

namespace Geex.Core.Notification
{
    public interface INotificationSender
    {
        Task SendNotificationAsync(string message);
    }

    public class NotificationSender : INotificationSender
    {
        public Task SendNotificationAsync(string message)
        {
            throw new Exception("todo");
        }
    }
}