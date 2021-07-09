using System;
using System.Security.Claims;
using Geex.Core.Authentication.Domain;
using HotChocolate.Types;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Geex.Core.Authentication.GqlSchemas.Types
{
    public class UserTokenGqlType : ObjectType<UserToken>
    {
        protected override void Configure(IObjectTypeDescriptor<UserToken> descriptor)
        {
            descriptor.BindFieldsImplicitly();
            base.Configure(descriptor);
        }
    }
}
