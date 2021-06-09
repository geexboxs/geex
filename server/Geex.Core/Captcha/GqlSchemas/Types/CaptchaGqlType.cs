using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geex.Core.Captcha.Domain;
using Geex.Shared._ShouldMigrateToLib;

using HotChocolate.Types;

namespace Geex.Core.Captcha.GqlSchemas.Types
{
    public class CaptchaGqlType : ObjectType<Domain.Captcha>
    {
        protected override void Configure(IObjectTypeDescriptor<Domain.Captcha> descriptor)
        {

            descriptor.BindFieldsExplicitly();
            descriptor.Field(x => x.CaptchaType);
            descriptor.Field(x => x.Key);
            descriptor.Field((ImageCaptcha x) => x.Bitmap).Use(next => async context =>
            {
                await next(context);
                if (context.Result is MemoryStream stream)
                {
                    context.Result = Convert.ToBase64String(stream.ToArray());
                }
            }).Type<StringType>();
            base.Configure(descriptor);
        }
    }
}
