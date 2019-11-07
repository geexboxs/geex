using System;
using Geex.Core.User;
using Geex.Shared;


namespace Geex.Core
{
    [DependsOn(typeof(UserModule))]
    public class AppModule : GraphQLEntryModule<AppModule>
    {

        public override void PostInitialize(IServiceProvider serviceProvider)
        {
        }
    }
}
