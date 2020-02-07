using System;
using Autofac;
using Geex.Shared;
using Geex.Shared._ShouldMigrateToLib.Auth;
using HotChocolate;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Repository.Mongo;

namespace Geex.Core.Users
{
    public class UserModule : GraphQLModule<UserModule>
    {

        public override void PostInitialize(IComponentContext serviceProvider)
        {
            return;
        }

        public override void PreInitialize(ContainerBuilder containerBuilder, ISchemaBuilder schemaBuilder)
        {
            base.PreInitialize(containerBuilder, schemaBuilder);
            containerBuilder.Register(x=> new Repository<AppUser>(x.Resolve<IMongoDatabase>(),"Users"));
            containerBuilder.Register(x => new Repository<AuthUser>(x.Resolve<IMongoDatabase>(), "Users"));
        }
    }
}
