using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Geex.Core.Users.Inputs;
using Geex.Shared.Roots;
using HotChocolate;
using IdentityModel;
using IdentityModel.Client;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Geex.Core.Users
{
    [GraphQLResolverOf(typeof(User))]
    [GraphQLResolverOf(typeof(Query))]
    public class UserResolver
    {
        [GraphQLDescription("This field does ...")]
        public IQueryable<User> QueryUsers([Parent] Query query, [Service]IMongoCollection<User> userCollection)
        {
            return userCollection.AsQueryable();
        }



        public async Task<bool> Register([Parent] Mutation mutation, [Service]IMongoCollection<User> userCollection, [Service]IPasswordHasher<User> passwordHasher, RegisterUserInput input)
        {
            await userCollection.InsertOneAsync(new User(input.PhoneOrEmail, input.Password, userCollection.AsQueryable(), (u, x) => passwordHasher.HashPassword(u, x), input.UserName));
            return true;
        }

        public async Task<bool> Authenticate([Parent] Mutation mutation,
            [Service]IMongoCollection<User> userCollection,
            [Service]IHttpContextAccessor httpContextAccessor,
            [Service]IPasswordHasher<User> passwordHasher,
            [Service]IIdentityServerInteractionService identityServerInteractionService,
            AuthenticateInput input)
        {
            var user = (await userCollection.FindAsync(x => (x.Username == input.UserIdentifier || x.PhoneNumber == input.UserIdentifier || x.Email == input.UserIdentifier))).First();
            if (passwordHasher.VerifyHashedPassword(user, user.Password, input.Password) != PasswordVerificationResult.Failed &&
                identityServerInteractionService.IsValidReturnUrl(input.RedirectUri))
            {
                await httpContextAccessor.HttpContext.SignInAsync(new IdentityServerUser(user.Id.ToString()), new AuthenticationProperties()
                {
                    AllowRefresh = false,
                    ExpiresUtc = DateTimeOffset.Now.AddHours(2),
                    IsPersistent = true,
                    IssuedUtc = DateTimeOffset.Now,
                    RedirectUri = input.RedirectUri,
                    Items = { { "roles", user.Roles.Select(x => x.Name).ToJson() } }
                });
            }

            return true;
        }

        [Authorize(Policy = "Permission.Mutation.AssignRoles")]
        public async Task<bool> AssignRoles([Parent] Mutation mutation, [Service]IMongoCollection<User> userCollection, AssignRoleInput input)
        {
            var user = await userCollection.Find(x => x.Id == ObjectId.Parse(input.UserId)).FirstOrDefaultAsync();
            user.Roles.Clear();
            foreach (var role in input.Roles)
            {
                user.Roles.Add(new Role(role));
            }
            var updateDefinition = new UpdateDefinitionBuilder<User>().Set(x => x.Roles, user.Roles);
            userCollection.UpdateOne(x => x.Id == user.Id, updateDefinition);
            return true;
        }
    }
}