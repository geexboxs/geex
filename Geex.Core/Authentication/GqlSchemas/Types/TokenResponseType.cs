using HotChocolate.Types;
using IdentityModel.Client;
using IdentityServer4.Events;
using IdentityServer4.Models;

namespace Geex.Core.Authentication.GqlSchemas.Types
{
    public class TokenResponseType : ObjectType<TokenIssuedSuccessEvent.Token>
    {
        protected override void Configure(IObjectTypeDescriptor<TokenIssuedSuccessEvent.Token> descriptor)
        {
            base.Configure(descriptor);
        }
    }
}
