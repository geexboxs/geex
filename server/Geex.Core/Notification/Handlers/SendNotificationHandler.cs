using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Geex.Core.Captcha.Commands;

using MediatR;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Volo.Abp.DependencyInjection;

namespace Geex.Core.Notification.Handlers
{
    public class SendNotificationHandler : IRequestHandler<SendSmsCaptchaRequest, Unit>, ITransientDependency
    {
        public ISmsNotificationSender Sender { get; }
        public ILogger<SendNotificationHandler> Logger { get; }

        public SendNotificationHandler(ISmsNotificationSender sender, ILogger<SendNotificationHandler> logger)
        {
            Sender = sender;
            Logger = logger;
        }
        public async Task<Unit> Handle(SendSmsCaptchaRequest request, CancellationToken cancellationToken)
        {
            await Sender.SendSmsNotificationAsync(request.PhoneNumber, SmsTemplateType.Captcha, request.Captcha);
            return Unit.Value;
        }
    }
}
