using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geex.Core.Captcha.Domain;
using Geex.Core.Captcha.GqlSchemas.Inputs;
using Geex.Shared.Roots;

using HotChocolate;
using HotChocolate.Types;

namespace Geex.Core.Captcha
{
    [ExtendObjectType(nameof(Mutation))]
    public class UserMutation : Mutation
    {
        public async Task<string> SendCaptcha([Parent] Mutation mutation,
            SendCaptchaInput input)
        {
            string captcha = String.Empty;
            if (input.CaptchaType == CaptchaType.Sms)
            {
                captcha = DateTimeOffset.Now.Ticks.ToString();
            }
            return captcha;
        }
    }
}
