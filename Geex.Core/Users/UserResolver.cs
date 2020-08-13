using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Autofac;
using Geex.Core.Users.GqlSchemas.Inputs;
using Geex.Shared._ShouldMigrateToLib;
using Geex.Shared._ShouldMigrateToLib.Auth;
using Geex.Shared.Roots;
using HotChocolate;
using IdentityModel;
using IdentityModel.Client;
using IdentityServer4;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using Volo.Abp.Domain.Repositories;

namespace Geex.Core.Users
{
    [GraphQLResolverOf(typeof(AppUser))]
    [GraphQLResolverOf(typeof(Query))]
    public class UserResolver
    {
        [GraphQLDescription("This field does ...")]
        public IQueryable<AppUser> QueryUsers([Parent] Query query, [Service]IRepository<AppUser> userCollection)
        {
            return userCollection;
        }
        public async Task<bool> Register([Parent] Mutation mutation,
            [Service]IComponentContext componentContext,
            RegisterUserInput input)
        {
            var user = new AuthUser(input.PhoneOrEmail, input.Password, input.UserName);
            var authUserCollection = componentContext.Resolve<IRepository<AuthUser>>();
            await authUserCollection.InsertAsync(user);
            var appUser = new AppUser(user);
            var appUserCollection = componentContext.Resolve<IRepository<AppUser>>();
            await appUserCollection.InsertAsync(appUser);
            return true;
        }

        


        public async Task<bool> AssignRoles([Parent] Mutation mutation, [Service]IRepository<AppUser> userCollection, AssignRoleInput input)
        {
            var user = await userCollection.GetAsync(x=>x.Id == input.UserId);
            user.Roles.Clear();
            foreach (var role in input.Roles)
            {
                user.Roles.Add(new Role(role));
            }
            await userCollection.UpdateAsync(user);
            return true;
        }
    }
}