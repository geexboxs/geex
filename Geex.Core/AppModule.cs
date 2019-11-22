using System;
using Autofac;
using Geex.Core.UserManagement;
using Geex.Shared;
using HotChocolate;
using Microsoft.Extensions.DependencyInjection;


namespace Geex.Core
{
    [DependsOn(typeof(UserModule))]
    public class AppModule : GraphQLEntryModule<AppModule>
    {
        public override void PreInitialize(ContainerBuilder containerBuilder, SchemaBuilder schemaBuilder)
        {
            base.PreInitialize(containerBuilder, schemaBuilder);
        }

        public override void PostInitialize(IComponentContext serviceProvider)
        {
        }
    }
}
