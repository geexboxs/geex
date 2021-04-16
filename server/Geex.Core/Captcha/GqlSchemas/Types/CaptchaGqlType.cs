using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Geex.Shared._ShouldMigrateToLib;

using HotChocolate.Types;

namespace Geex.Core.Captcha.GqlSchemas.Types
{
    public class CaptchaGqlType : ObjectType<Geex.Shared._ShouldMigrateToLib.Captcha>
    {
        protected override void Configure(IObjectTypeDescriptor<Geex.Shared._ShouldMigrateToLib.Captcha> descriptor)
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
