using Geex.Shared;
using Geex.Shared._ShouldMigrateToLib;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
            context.Services.TryAddTransient(typeof(IGeexRedisClient), x => new GeexRedisClient(x.GetRequiredService<IDistributedCache>()));
            base.ConfigureServices(context);
        }
    }
}
