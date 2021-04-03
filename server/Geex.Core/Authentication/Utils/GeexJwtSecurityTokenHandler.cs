using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

using Geex.Shared._ShouldMigrateToLib;

using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace Geex.Core.Authentication.Utils
{
    public class GeexJwtSecurityTokenHandler : ISecurityTokenValidator
    {
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public GeexJwtSecurityTokenHandler()
        {
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public bool CanValidateToken => true;

        public int MaximumTokenSizeInBytes { get; set; } = TokenValidationParameters.DefaultMaximumTokenSizeInBytes;

        public bool CanReadToken(string securityToken)
        {
            return _tokenHandler.CanReadToken(securityToken);
        }

        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {

            var principal = _tokenHandler.ValidateToken(securityToken, validationParameters, out validatedToken);
            //if (validatedToken.ValidFrom > DateTime.Now && validatedToken.ValidTo < DateTime.Now)
            //{
            //    throw new SecurityTokenExpiredException($"token is only valid in {validatedToken.ValidFrom} ~ {validatedToken.ValidTo}");
            //}
            //var subClaim = principal.Claims.First(c => c.Type == GeexClaimType.Sub);
            //var expireClaim = principal.Claims.First(x => x.Type == GeexClaimType.Expires);

            return principal;
        }
    }

}
