using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geex.Shared;
using Volo.Abp.AspNetCore;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace Geex.Core.Authorization
{
    [DependsOn(
        typeof(AbpAspNetCoreModule),
        typeof(AbpUnitOfWorkModule)
    )]
    public class AuthorizationModule:GraphQLModule<AuthorizationModule>
    {
        
    }
}
