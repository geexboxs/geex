// Decompiled with JetBrains decompiler
// Type: System.Security.Principal.AbpClaimsIdentityExtensions
// Assembly: Volo.Abp.Security, Version=4.2.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B09FD2E-5972-45AF-98A6-99487C7BB7DD
// Assembly location: C:\Users\lulus\.nuget\packages\volo.abp.security\4.2.2\lib\netstandard2.0\Volo.Abp.Security.dll

using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

using Geex.Shared._ShouldMigrateToLib;

using Volo.Abp;

namespace System.Security.Principal
{
    public static class AbpClaimsIdentityExtensions
    {
        public static string? FindUserId(this ClaimsPrincipal principal)
        {
            Check.NotNull(principal, nameof(principal));
            IEnumerable<Claim> claims = principal.Claims;
            Claim? claim = claims != null
                ? claims.FirstOrDefault((Func<Claim, bool>)(c => c.Type == GeexClaimType.Sub))
                : null;
            if (claim == null || claim.Value.IsNullOrWhiteSpace())
                return default;
            Guid result;
            return claim?.Value;
        }

        public static string? FindUserId(this IIdentity identity)
        {
            Check.NotNull(identity, nameof(identity));
            Claim? claim;
            if (!(identity is ClaimsIdentity claimsIdentity))
            {
                claim = null;
            }
            else
            {
                IEnumerable<Claim> claims = claimsIdentity.Claims;
                claim = claims != null
                    ? claims.FirstOrDefault((Func<Claim, bool>)(c => c.Type == GeexClaimType.Sub))
                    : null;
            }

            return claim?.Value;
        }

        public static Guid? FindTenantId(this ClaimsPrincipal principal)
        {
            throw new NotImplementedException();
            //Check.NotNull(principal, nameof(principal));
            //IEnumerable<Claim> claims = principal.Claims;
            //Claim claim = claims != null
            //    ? claims.FirstOrDefault((Func<Claim, bool>) (c => c.Type == GeexClaimType.TenantId))
            //    : null;
            //return claim == null || claim.Value.IsNullOrWhiteSpace() ? new Guid?() : Guid.Parse(claim.Value);
        }

        public static Guid? FindTenantId(this IIdentity identity)
        {
            throw new NotImplementedException();
            //Check.NotNull(identity, nameof(identity));
            //Claim claim1;
            //if (!(identity is ClaimsIdentity claimsIdentity))
            //{
            //    claim1 = null;
            //}
            //else
            //{
            //    IEnumerable<Claim> claims = claimsIdentity.Claims;
            //    claim1 = claims != null
            //        ? claims.FirstOrDefault((Func<Claim, bool>) (c => c.Type == GeexClaimType.TenantId))
            //        : null;
            //}

            //Claim claim2 = claim1;
            //return claim2 == null || claim2.Value.IsNullOrWhiteSpace() ? new Guid?() : Guid.Parse(claim2.Value);
        }

        public static string FindClientId(this ClaimsPrincipal principal)
        {
            throw new NotImplementedException();
            //Check.NotNull(principal, nameof(principal));
            //IEnumerable<Claim> claims = principal.Claims;
            //Claim claim = claims != null
            //    ? claims.FirstOrDefault((Func<Claim, bool>) (c => c.Type == GeexClaimType.ClientId))
            //    : null;
            //return claim == null || claim.Value.IsNullOrWhiteSpace() ? null : claim.Value;
        }

        public static string FindClientId(this IIdentity identity)
        {
            throw new NotImplementedException();
            //Check.NotNull(identity, nameof(identity));
            //Claim claim1;
            //if (!(identity is ClaimsIdentity claimsIdentity))
            //{
            //    claim1 = null;
            //}
            //else
            //{
            //    IEnumerable<Claim> claims = claimsIdentity.Claims;
            //    claim1 = claims != null
            //        ? claims.FirstOrDefault((Func<Claim, bool>) (c => c.Type == GeexClaimType.ClientId))
            //        : null;
            //}

            //Claim claim2 = claim1;
            //return claim2 == null || claim2.Value.IsNullOrWhiteSpace() ? null : claim2.Value;
        }

        public static Guid? FindEditionId(this ClaimsPrincipal principal)
        {
            throw new NotImplementedException();
            //Check.NotNull(principal, nameof(principal));
            //IEnumerable<Claim> claims = principal.Claims;
            //Claim claim = claims != null
            //    ? claims.FirstOrDefault((Func<Claim, bool>) (c => c.Type == GeexClaimType.EditionId))
            //    : null;
            //return claim == null || claim.Value.IsNullOrWhiteSpace() ? new Guid?() : Guid.Parse(claim.Value);
        }

        public static Guid? FindEditionId(this IIdentity identity)
        {
            throw new NotImplementedException();
            //Check.NotNull(identity, nameof(identity));
            //Claim claim1;
            //if (!(identity is ClaimsIdentity claimsIdentity))
            //{
            //    claim1 = null;
            //}
            //else
            //{
            //    IEnumerable<Claim> claims = claimsIdentity.Claims;
            //    claim1 = claims != null
            //        ? claims.FirstOrDefault((Func<Claim, bool>) (c => c.Type == GeexClaimType.EditionId))
            //        : null;
            //}

            //Claim claim2 = claim1;
            //return claim2 == null || claim2.Value.IsNullOrWhiteSpace() ? new Guid?() : Guid.Parse(claim2.Value);
        }
    }
}