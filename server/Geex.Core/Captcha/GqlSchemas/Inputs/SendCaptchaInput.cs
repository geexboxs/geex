using Geex.Common.Gql.Types.Scalars;
using Geex.Core.Captcha.Domain;
using HotChocolate;
using HotChocolate.Types;

namespace Geex.Core.Captcha.GqlSchemas.Inputs
{
    public record SendCaptchaInput
    {
        public CaptchaProvider CaptchaProvider { get; set; }
        [GraphQLType(typeof(ChinesePhoneNumberType))]
        public string SmsCaptchaPhoneNumber { get; set; }
    }
}