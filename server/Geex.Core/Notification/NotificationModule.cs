using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geex.Common.Abstractions;
using Geex.Shared;

using Microsoft.Extensions.DependencyInjection;

using Volo.Abp.Modularity;

namespace Geex.Core.Notification
{
    [DependsOn(
    )]
    public class NotificationModule : GeexModule<NotificationModule>
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            var services = context.Services;
            services.AddSingleton<ISiteMessageNotificationSender, SiteMessageNotificationSender>();
            base.PreConfigureServices(context);
        }
    }
}
