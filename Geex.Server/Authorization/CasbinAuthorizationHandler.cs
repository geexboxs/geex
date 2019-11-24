using System.Threading.Tasks;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Driver;

namespace Geex.Core.Users
{
    public class CasbinAuthorizationHandler : AuthorizationHandler<CasbinRequirement>
    {
        private readonly Enforcer _enforcer;
        private readonly IMongoCollection<User> _userCollection;

        public CasbinAuthorizationHandler(Enforcer enforcer, IMongoCollection<User> userCollection)
        {
            _enforcer = enforcer;
            this._userCollection = userCollection;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CasbinRequirement requirement)
        {
            var obj = requirement.Obj ?? "*"; // the resource that is going to be accessed.
            var act = requirement.Act ?? "*"; // the operation that the user performs on the resource.
            if (_enforcer.Enforce(context.User.GetSubjectId(), obj, act))
            {
                // permit alice to read data1
                context.Succeed(requirement);
            }
            else
            {
                // deny the request, show an error
                context.Fail();
            }

            return;
        }
    }
}