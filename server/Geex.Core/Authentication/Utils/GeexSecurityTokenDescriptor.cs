using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Geex.Common.Abstractions;
using Geex.Common.Abstractions.Enumerations;
using Geex.Core.Authentication.Domain;
using Geex.Shared._ShouldMigrateToLib;

using Microsoft.IdentityModel.Tokens;

namespace Geex.Core.Authentication.Utils
{
    public class GeexSecurityTokenDescriptor : SecurityTokenDescriptor
    {
        public GeexSecurityTokenDescriptor(User user, LoginProvider provider, UserTokenGenerateOptions options)
        {
            this.Audience = options.Audience;
            this.SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey)), SecurityAlgorithms.HmacSha256Signature);
            Expires = DateTime.Now.Add(options.Expires);
            IssuedAt = DateTime.Now;
            Issuer = options.Issuer;
            Subject = new ClaimsIdentity(new Claim[]
            {
                new GeexClaim(GeexClaimType.Sub, user.Id),
                new GeexClaim(GeexClaimType.Provider, provider),
            });
            Claims = user.Claims.ToDictionary(x => x.ClaimType, x => (object)x.ClaimValue);
        }
    }
}