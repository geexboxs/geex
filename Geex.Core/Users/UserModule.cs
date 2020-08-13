using System;

using Autofac;

using Geex.Shared;
using Geex.Shared._ShouldMigrateToLib.Auth;

using HotChocolate;

using Microsoft.Extensions.DependencyInjection;

using MongoDB.Driver;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;

namespace Geex.Core.Users
{
    public class UserModule : GraphQLModule<UserModule>
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            base.ConfigureServices(context);
        }
    }
}
