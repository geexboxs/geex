using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Geex.Core.Authentication.Domain;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Driver;
using NetCasbin;

namespace Geex.Core.Authorization.Casbin
{
    public class CasbinAuthorizationHandler : AuthorizationHandler<CasbinRequirement>
    {
        private readonly Enforcer _enforcer;

        public CasbinAuthorizationHandler(Enforcer enforcer)
        {
            _enforcer = enforcer;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CasbinRequirement requirement)
        {
            var obj = requirement.Obj ?? "*"; // the resource that is going to be accessed.
            var act = requirement.Act ?? "*"; // the operation that the user performs on the resource.
            if (await _enforcer.EnforceAsync(context.User.FindUserId(), obj, act))
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
