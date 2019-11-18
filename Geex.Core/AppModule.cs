using System;
using Geex.Core.UserManagement;
using Geex.Shared;
using HotChocolate;
using Microsoft.Extensions.DependencyInjection;


namespace Geex.Core
{
    [DependsOn(typeof(UserModule))]
    public class AppModule : GraphQLEntryModule<AppModule>
    {
        public override void PreInitialize(IServiceCollection containerBuilder, SchemaBuilder schemaBuilder)
        {
            base.PreInitialize(containerBuilder, schemaBuilder);
        }

        public override void PostInitialize(IServiceProvider serviceProvider)
        {
        }
    }
}
