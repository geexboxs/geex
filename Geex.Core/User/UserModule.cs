using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geex.Core.User.Types;
using Geex.Core.User.Types.RootExtensions;
using Geex.Shared;
using HotChocolate;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Geex.Core.User
{
    public class UserModule : GraphQLModule<UserModule>
    {

        public override void PostInitialize()
        {
        }

        public override void PreInitialize(IServiceCollection containerBuilder, SchemaBuilder schemaBuilder)
        {
            base.PreInitialize(containerBuilder, schemaBuilder);
        }
    }
}
