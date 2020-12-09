using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Autofac;

using Geex.Core.Users;
using Geex.Shared._ShouldMigrateToLib.Abstractions;
using Geex.Shared._ShouldMigrateToLib.Auth;
using Geex.Shared.Roots;

using HotChocolate;

using IdentityServer4.Models;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

using MongoDB.Bson;
using Volo.Abp;

namespace Geex.Core.Authorization
{
    //[GraphQLResolverOf(typeof(Query))]
    //public class AuthorizationResolver
    //{
    //    public async Task<bool> Authorize([Parent] Mutation mutation,
    //        [Service] IComponentContext componentContext,
    //        AuthorizeInput input)
    //    {
    //        if (input.AuthorizeTargetType == AuthorizeTargetType.Role)
    //        {
    //            var role = await IActiveRecord<Role>.StaticRepository.GetAsync(input.TargetId);
    //            role.AuthorizedPermissions = input.AllowedPermissions;
    //            return true;
    //        }

    //        if (input.AuthorizeTargetType == AuthorizeTargetType.Role)
    //        {
    //            var user = await IActiveRecord<User>.StaticRepository.GetAsync(input.TargetId);
    //            user.AuthorizedPermissions = input.AllowedPermissions;
    //            return true;
    //        }

    //        throw new UserFriendlyException("todo");
    //    }
    //}

    //public class AuthorizeTargetType : Enumeration<string>
    //{
    //    public static AuthorizeTargetType Role { get; set; }
    //    public static AuthorizeTargetType User { get; set; }
    //}

    //public class AuthorizeInput
    //{
    //    public AuthorizeTargetType AuthorizeTargetType { get; set; }
    //    public List<AppPermission> AllowedPermissions { get; set; }
    //    public ObjectId TargetId { get; set; }
    //}

    public class AppPermission/* : Enumeration<string>*/
    {
    }
}
