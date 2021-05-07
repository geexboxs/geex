using Geex.Shared;
using Geex.Shared._ShouldMigrateToLib;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Volo.Abp.Modularity;

namespace Geex.Core.Captcha
{
    [DependsOn(
    )]
    public class CaptchaModule : GraphQLModule<CaptchaModule>
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            base.ConfigureServices(context);
        }
    }
}
