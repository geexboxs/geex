using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Autofac;
using Geex.Core.Users.Inputs;
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
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using Repository.Mongo;

namespace Geex.Core.Users
{
    [GraphQLResolverOf(typeof(AppUser))]
    [GraphQLResolverOf(typeof(Query))]
    public class UserResolver
    {
        [GraphQLDescription("This field does ...")]
        public IQueryable<AppUser> QueryUsers([Parent] Query query, [Service]Repository<AppUser> userCollection)
        {
            return userCollection.Collection.AsQueryable();
        }
        public async Task<bool> Register([Parent] Mutation mutation,
            [Service]IComponentContext componentContext,
            RegisterUserInput input)
        {
            var user = new AuthUser(input.PhoneOrEmail, input.Password, input.UserName);
            var authUserCollection = componentContext.Resolve<Repository<AuthUser>>();
            authUserCollection.Insert(user);
            var appUser = new AppUser(user);
            var appUserCollection = componentContext.Resolve<Repository<AppUser>>();
            appUserCollection.Insert(appUser);
            return true;
        }

        


        public async Task<bool> AssignRoles([Parent] Mutation mutation, [Service]Repository<AppUser> userCollection, AssignRoleInput input)
        {
            var user = userCollection.Get(input.UserId);
            user.Roles.Clear();
            foreach (var role in input.Roles)
            {
                user.Roles.Add(new Role(role));
            }
            userCollection.Update(user, x => x.Roles, user.Roles);
            return true;
        }
    }
}