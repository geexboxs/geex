using Geex.Shared;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace Geex.Core.Captcha
{
    [DependsOn(
        typeof(AbpAutofacModule)
    )]
    public class CaptchaModule : GraphQLModule<CaptchaModule>
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            base.ConfigureServices(context);
        }
    }
}
