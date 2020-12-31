using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Autofac;

using Geex.Core.Authentication.Domain;
using Geex.Core.Users;
using Geex.Shared._ShouldMigrateToLib.Abstractions;
using Geex.Shared._ShouldMigrateToLib.Auth;
using Geex.Shared.Roots;

using HotChocolate;
using HotChocolate.AspNetCore.Authorization;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

using MongoDB.Bson;
using MongoDB.Entities;

using Volo.Abp;

namespace Geex.Core.Authorization
{
    [GraphQLResolverOf(typeof(Query))]
    public class AuthorizationResolver
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

    public class AuthorizeTargetType : Enumeration<AuthorizeTargetType, string>
    {
        public static AuthorizeTargetType Role { get; set; }
        public static AuthorizeTargetType User { get; set; }

        public AuthorizeTargetType(string value) : base(value)
        {
        }
    }

    public class AuthorizeInput
    {
        public AuthorizeTargetType AuthorizeTargetType { get; set; }
        public List<AppPermission> AllowedPermissions { get; set; }
        public ObjectId TargetId { get; set; }
    }

    public class AppPermission : Enumeration<AppPermission, string>
    {
        public AppPermission(string value) : base(value)
        {
        }

        public const string _AssignRole = nameof(AssignRole);
        public static AppPermission AssignRole { get; } = new(_AssignRole);
    }
}
