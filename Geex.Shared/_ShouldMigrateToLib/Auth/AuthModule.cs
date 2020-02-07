using System;
using System.Collections.Generic;
using System.Text;
using Autofac;

namespace Geex.Shared._ShouldMigrateToLib.Auth
{
    public class AuthModule:GraphQLModule<AuthModule>
    {
        public override void PostInitialize(IComponentContext serviceProvider)
        {
            return;
        }
    }
}
