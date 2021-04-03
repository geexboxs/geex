using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Autofac;

using Geex.Core.Authentication.Domain;
using Geex.Core.Users;
using Geex.Shared._ShouldMigrateToLib.Auth;
using Geex.Shared.Roots;

using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Resolvers;
using HotChocolate.Types;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

using MongoDB.Entities;

using Volo.Abp;

namespace Geex.Core.Authorization
{
    [ExtendObjectType(nameof(Mutation))]
    public class AuthorizationMutation : Mutation
    {
        public async Task<bool> Authorize([Parent] Mutation mutation,
            [Service] IComponentContext componentContext,
            AuthorizeInput input)
        {
            var enforcer = componentContext.Resolve<RbacEnforcer>();
            await enforcer.SetPermissionsAsync(input.TargetId.ToString(), input.AllowedPermissions.ToArray());
            return true;
        }
    }
}
