using Microsoft.AspNetCore.Authorization;

namespace Geex.Core.Users
{
    public class CasbinRequirement : IAuthorizationRequirement
    {
        public string Obj { get; }
        public string Act { get; }

        public CasbinRequirement(string obj, string act)
        {
            Obj = obj;
            Act = act;
        }

        public CasbinRequirement(string policyName)
        {
            this.Obj = policyName;
        }
    }
}