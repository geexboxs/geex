using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Geex.Core.Notification
{
    public interface ISiteMessageNotificationSender
    {
        Task SendNotificationAsync(string message);
    }
    [Dependency(ServiceLifetime.Transient)]
    public class SiteMessageNotificationSender : ISiteMessageNotificationSender
    {
        public Task SendNotificationAsync(string message)
        {
            throw new Exception("todo");
        }
    }
}