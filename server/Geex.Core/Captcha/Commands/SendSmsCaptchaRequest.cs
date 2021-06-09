using Geex.Core.Captcha.Domain;
using Geex.Shared._ShouldMigrateToLib;

using MediatR;

namespace Geex.Core.Captcha.Commands
{
    public record SendSmsCaptchaRequest : IRequest<Unit>
    {
        public string PhoneNumber { get; }
        public SmsCaptcha Captcha { get; }

        public SendSmsCaptchaRequest(string phoneNumber, SmsCaptcha captcha)
        {
            PhoneNumber = phoneNumber;
            Captcha = captcha;
        }
    }
}