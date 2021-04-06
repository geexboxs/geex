using Geex.Shared._ShouldMigrateToLib.Abstractions;
using JetBrains.Annotations;

namespace Geex.Core.Captcha.Domain
{
    public class CaptchaProvider : Enumeration<CaptchaProvider, string>
    {
        public CaptchaProvider([NotNull] string name, string value) : base(name, value)
        {
        }

        public CaptchaProvider(string value) : base(value)
        {
        }

        public static readonly CaptchaProvider Sms = new CaptchaProvider(nameof(Sms));
        public static readonly CaptchaProvider Image = new CaptchaProvider(nameof(Image));
    }
}