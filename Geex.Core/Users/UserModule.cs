using System;

using Autofac;

using Geex.Shared;
using Geex.Shared._ShouldMigrateToLib.Auth;

using HotChocolate;

using Microsoft.Extensions.DependencyInjection;

using MongoDB.Driver;
using Volo.Abp.AspNetCore;
using Volo.Abp.Autofac;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;
using Volo.Abp.Uow;

namespace Geex.Core.Users
{
    [DependsOn(
        typeof(AbpAspNetCoreModule),
        typeof(AbpAutofacModule),
        typeof(AbpUnitOfWorkModule),
        typeof(AbpMongoDbModule)
    )]
    public class UserModule : GraphQLModule<UserModule>
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMongoDbContext<UserDbContext>(options =>
            {
                options.AddDefaultRepositories();
            });
            base.ConfigureServices(context);
        }
    }
}
