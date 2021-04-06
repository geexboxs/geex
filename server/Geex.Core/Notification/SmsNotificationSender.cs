using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Geex.Core.Notification
{
    public interface ISmsNotificationSender
    {
        Task SendSmsNotificationAsync(string phoneNumber, SmsTemplateType smsTemplateType, object data);
    }

    public enum SmsTemplateType
    {
        Captcha
    }

    [Dependency(ServiceLifetime.Transient)]
    public class SmsNotificationSender : ISmsNotificationSender
    {
        public async Task SendSmsNotificationAsync(string phoneNumber, SmsTemplateType smsTemplateType, object data)
        {
            // todo
        }
    }
}
