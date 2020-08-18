using HotChocolate.Types;
using IdentityModel.Client;
using IdentityServer4.Events;
using IdentityServer4.Models;

namespace Geex.Core.Authentication.GqlSchemas.Types
{
    public class AccessTokenType : ObjectType<TokenResponse>
    {
        protected override void Configure(IObjectTypeDescriptor<TokenResponse> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Field(x => x.AccessToken);
            descriptor.Field(x => x.ExpiresIn);
            descriptor.Field(x => x.IdentityToken);
            descriptor.Field(x => x.RefreshToken);
            descriptor.Field(x => x.TokenType);
            descriptor.Field(x => x.IsError);
            descriptor.Field(x => x.Error);
        }
    }
}
