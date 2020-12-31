using System;
using System.Security.Claims;
using System.Text;
using Geex.Core.Authentication.Domain;
using Geex.Shared._ShouldMigrateToLib;
using Microsoft.IdentityModel.Tokens;

namespace Geex.Core.Authentication.Utils
{
    public class GeexSecurityTokenDescriptor : SecurityTokenDescriptor
    {
        public GeexSecurityTokenDescriptor(User user, LoginProvider provider, UserTokenGenerateOptions options)
        {
            this.Audience = "*";
            this.SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey)), SecurityAlgorithms.HmacSha256Signature);
            Expires = DateTime.Now.Add(options.Expires);
            IssuedAt = DateTime.Now;
            Subject = new ClaimsIdentity(new Claim[]
            {
                new GeexClaim(GeexClaimType.Sub, user.ID),
                new GeexClaim(GeexClaimType.UserName, user.UserName),
                new GeexClaim(GeexClaimType.Provider, provider),
            });
        }
    }
}