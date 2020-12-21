using System;
using System.Security.Claims;
using HotChocolate.Types;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Geex.Core.Authentication.GqlSchemas.Types
{
    public class ClaimsIdentityGqlType : ObjectType<ClaimsIdentity>
    {
        protected override void Configure(IObjectTypeDescriptor<ClaimsIdentity> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Field(x => x);
            base.Configure(descriptor);
        }
    }
}
