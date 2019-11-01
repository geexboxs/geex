using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geex.Shared;
using Microsoft.Extensions.Logging;

namespace Geex.Core.User
{
    public class UserModule : GraphQLModule<UserModule>
    {
        public UserModule(ILogger<UserModule> logger) : base(logger)
        {
        }

        public override void PreInitialize()
        {
            throw new NotImplementedException();
        }

        public override void OnInitialize()
        {
            throw new NotImplementedException();
        }

        public override void PostInitialize()
        {
            throw new NotImplementedException();
        }
    }
}
