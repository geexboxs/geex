using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geex.Core.Captcha.Commands;
using Geex.Core.Captcha.Domain;
using Geex.Core.Captcha.GqlSchemas.Inputs;
using Geex.Shared._ShouldMigrateToLib;
using Geex.Shared.Roots;
using Geex.Shared.Types.Scalars;
using HotChocolate;
using HotChocolate.Types;

using MediatR;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace Geex.Core.Captcha
{
    [ExtendObjectType(nameof(Mutation))]
    public class UserMutation : Mutation
    {
        public async Task<Shared._ShouldMigrateToLib.Captcha> GenerateCaptcha([Parent] Mutation mutation,
            [Service] IGeexRedisClient cache,
            [Service] IMediator mediator,
            SendCaptchaInput input)
        {
            cache = cache.SwitchNamespace(RedisNamespace.Captcha);
            if (input.CaptchaProvider == CaptchaProvider.Sms)
            {
                var captcha = new SmsCaptcha();
                await cache.SetAsync(captcha.Key, captcha);
                await mediator.Send(new SendSmsCaptchaRequest(input.SmsCaptchaPhoneNumber, captcha));
                return captcha;
            }

            if (input.CaptchaProvider == CaptchaProvider.Image)
            {
                var captcha = new ImageCaptcha();
                await cache.SetAsync(captcha.Key, captcha);
                return captcha;
            }
            throw new ArgumentOutOfRangeException("input.CaptchaProvider");
        }

        public async Task ValidateCaptcha([Parent] Mutation mutation,
            [Service] IGeexRedisClient cache,
            ValidateCaptchaInput input)
        {
            cache = cache.SwitchNamespace(RedisNamespace.Captcha);
            if (input.CaptchaProvider == CaptchaProvider.Sms)
            {
                throw new Exception("todo");
            }

            if (input.CaptchaProvider == CaptchaProvider.Image)
            {
                var captcha = await cache.GetAsync<ImageCaptcha>(input.CaptchaKey);
                if (captcha.Code != input.CaptchaCode)
                {
                    throw new UserFriendlyException("invalid_captcha");
                }
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
