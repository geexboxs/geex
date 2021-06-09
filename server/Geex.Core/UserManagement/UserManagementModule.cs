using Geex.Common.Abstractions;
using Geex.Shared;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Geex.Core.UserManagement
{
    [DependsOn(
    )]
    public class UserManagementModule : GeexModule<UserManagementModule>
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            base.ConfigureServices(context);
        }
    }
}
