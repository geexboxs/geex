using Microsoft.AspNetCore.Authorization;

namespace Geex.Core.Users
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(string policyName)
        {
            this.RequiredPermission = policyName;
        }

        public string RequiredPermission { get; set; }
    }
}