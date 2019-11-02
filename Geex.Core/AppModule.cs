using System;
using Autofac;
using Geex.Core.User;
using UserEntity = Geex.Core.User.User;
using Geex.Shared;
using HotChocolate;
using Microsoft.Extensions.DependencyInjection;

namespace Geex.Core
{
    [DependsOn(typeof(UserModule))]
    public class AppModule : GraphQLEntryModule<AppModule>
    {

        public override void PostInitialize()
        {
        }
    }
}
