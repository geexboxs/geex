using Geex.Shared._ShouldMigrateToLib.Abstractions;
using JetBrains.Annotations;

namespace Geex.Core.Captcha.Domain
{
    public class CaptchaType : Enumeration<CaptchaType, string>
    {
        public CaptchaType([NotNull] string name, string value) : base(name, value)
        {
        }

        public CaptchaType(string value) : base(value)
        {
        }

        public static readonly CaptchaType Sms = new CaptchaType(nameof(Sms), nameof(Sms));
    }
}