using Geex.Core.Captcha.Domain;

namespace Geex.Core.Captcha.GqlSchemas.Inputs
{
    public record SendCaptchaInput
    {
        public CaptchaType CaptchaType { get; set; }
    }
}