using System;
using Autofac;
using Geex.Shared;
using HotChocolate;
using Microsoft.Extensions.DependencyInjection;

namespace Geex.Core.UserManagement
{
    public class UserModule : GraphQLModule<UserModule>
    {

        public override void PostInitialize(IContainer serviceProvider)
        {
        }

        public override void PreInitialize(ContainerBuilder containerBuilder, SchemaBuilder schemaBuilder)
        {
            base.PreInitialize(containerBuilder, schemaBuilder);
        }
    }
}
