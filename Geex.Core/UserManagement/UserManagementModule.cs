using Geex.Shared;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;
using Volo.Abp.Uow;

namespace Geex.Core.UserManagement
{
    [DependsOn(
        typeof(AbpAspNetCoreModule),
        typeof(AbpAutofacModule),
        typeof(AbpUnitOfWorkModule),
        typeof(AbpMongoDbModule)
    )]
    public class UserManagementModule : GraphQLModule<UserManagementModule>
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMongoDbContext<UserManagementDbContext>(options =>
            {
                options.AddDefaultRepositories();
            });
            base.ConfigureServices(context);
        }
    }
}
