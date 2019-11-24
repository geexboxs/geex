using System.Collections.Generic;
using System.Threading.Tasks;
using Geex.Shared._ShouldMigrateToLib;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace Geex.Core.Users
{
    public class CasbinAuthorizationPolicyProvider : IAuthorizationPolicyProvider
    {
        private readonly Enforcer _enforcer;
        public static HashSet<string> HardCodedPermissions { get; } = new HashSet<string>();
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        public CasbinAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options, Enforcer enforcer)
        {
            _enforcer = enforcer;
            // ASP.NET Core only uses one authorization policy provider, so if the custom implementation
            // doesn't handle all policies (including default policies, etc.) it should fall back to an
            // alternate provider.
            //
            // In this sample, a default authorization policy provider (constructed with options from the 
            // dependency injection container) is used if this custom provider isn't able to handle a given
            // policy name.
            //
            // If a custom policy provider is able to handle all expected policy names then, of course, this
            // fallback pattern is unnecessary.
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public async Task<AuthorizationPolicy> GetDefaultPolicyAsync() => await FallbackPolicyProvider.GetDefaultPolicyAsync();
        public async Task<AuthorizationPolicy> GetFallbackPolicyAsync() => await FallbackPolicyProvider.GetDefaultPolicyAsync();

        // Policies are looked up by string name, so expect 'parameters' (like age)
        // to be embedded in the policy names. This is abstracted away from developers
        // by the more strongly-typed attributes derived from AuthorizeAttribute
        // (like [MinimumAgeAuthorize] in this sample)
        public async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            var policy = new AuthorizationPolicyBuilder();
            var requirement = new PermissionRequirement(policyName);
            HardCodedPermissions.Add(policyName);

            policy.AddRequirements(requirement);
            return policy.Build();
        }
    }
}