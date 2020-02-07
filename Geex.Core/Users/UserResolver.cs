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
using Geex.Shared._ShouldMigrateToLib.Middlewares;
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
        [UnitOfWork]
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

        //public async Task<TokenResponse> Authenticate([Parent] Mutation mutation,
        //    [Service]IComponentContext componentContext,

        //    AuthenticateInput input)
        //{
        //    IMongoCollection<AppUser> userCollection = componentContext.Resolve<IMongoCollection<AppUser>>();
        //    IHttpContextAccessor httpContextAccessor = componentContext.Resolve<IHttpContextAccessor>();
        //    IPasswordHasher<AppUser> passwordHasher = componentContext.Resolve<IPasswordHasher<AppUser>>();
        //    IHostEnvironment env = componentContext.Resolve<IHostEnvironment>();
        //    if (httpContextAccessor.HttpContext.Request.Headers.TryGetValue("Authentication", out var authValue))
        //    {
        //        await httpContextAccessor.HttpContext.AuthenticateAsync();
        //    }
        //    var client = new HttpClient() { BaseAddress = new Uri($"http://{Environment.GetEnvironmentVariable("HOST_NAME")}") };
        //    var tokenResponse = await client.RequestTokenAsync(new TokenRequest()
        //    {
        //        Address = "http://localhost:8000/connect/token",
        //        ClientId = env.ApplicationName,
        //        ClientSecret = env.ApplicationName,
        //        GrantType = GrantType.ResourceOwnerPassword,
        //        Parameters = new Dictionary<string, string>() {
        //            { "username", input.UserIdentifier },
        //            { "password", input.Password },
        //            { "scope",$"{env.ApplicationName}" }
        //        }
        //    });
        //    if (tokenResponse.HttpStatusCode == HttpStatusCode.OK)
        //    {
        //        return tokenResponse;
        //    }
        //    throw tokenResponse.Exception;
        //}


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