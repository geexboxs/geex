using Geex.Core.UserManagement;
using Geex.Shared;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;

namespace Geex.Core.SystemSettings
{
    [DependsOn(
        typeof(AbpAutofacModule)
    )]
    public class SystemSettingModule : GraphQLModule<UserManagementModule>
    {

    }
}
