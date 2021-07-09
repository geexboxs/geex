using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Geex.Common.Gql.Roots;
using Geex.Core.Captcha.Commands;
using Geex.Core.Captcha.Domain;
using Geex.Core.Captcha.GqlSchemas.Inputs;
using Geex.Shared._ShouldMigrateToLib;

using HotChocolate;
using HotChocolate.Types;

using MediatR;

using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Core.Abstractions;


namespace Geex.Core.Captcha
{
    public class UserMutation : MutationTypeExtension<UserMutation>
    {
        public async Task<Domain.Captcha> GenerateCaptcha(
            [Service] IRedisDatabase cache,
            [Service] IMediator mediator,
            SendCaptchaInput input)
        {
            if (input.CaptchaProvider == CaptchaProvider.Sms)
            {
                IRedisCacheClient a;
                var captcha = new SmsCaptcha();
                await cache.SetNamedAsync(captcha);
                await mediator.Send(new SendSmsCaptchaRequest(input.SmsCaptchaPhoneNumber, captcha));
                return captcha;
            }

            if (input.CaptchaProvider == CaptchaProvider.Image)
            {
                var captcha = new ImageCaptcha();
                await cache.SetNamedAsync(captcha);
                return captcha;
            }
            throw new ArgumentOutOfRangeException("input.CaptchaProvider");
        }

        public async Task<bool> ValidateCaptcha(
            [Service] IRedisDatabase cache,
            ValidateCaptchaInput input)
        {
            if (input.CaptchaProvider == CaptchaProvider.Sms)
            {
                var captcha = await cache.GetAsync<SmsCaptcha>(input.CaptchaKey);
                if (captcha.Code != input.CaptchaCode)
                {
                    return false;
                }
                return true;
            }

            if (input.CaptchaProvider == CaptchaProvider.Image)
            {
                var captcha = await cache.GetAsync<ImageCaptcha>(input.CaptchaKey);
                if (captcha.Code != input.CaptchaCode)
                {
                    return false;
                }
                return true;
            }
            throw new ArgumentOutOfRangeException("input.CaptchaProvider");
        }
    }

    public class ValidateCaptchaInput
    {
        public string CaptchaKey { get; set; }
        public CaptchaProvider CaptchaProvider { get; set; }
        public string CaptchaCode { get; set; }
    }
}
