using System;
using System.Linq;
using System.Threading.Tasks;
using Geex.Core.UserManagement.Inputs;
using Geex.Shared.Roots;
using HotChocolate;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

namespace Geex.Core.UserManagement
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



        public async Task<bool> Register([Parent] Mutation mutation, [Service]IMongoCollection<User> userCollection, RegisterUserInput input)
        {
            await userCollection.InsertOneAsync(new User(input.PhoneOrEmail, input.Password, input.UserName,));
            return true;
        }

        public async Task<bool> AssignRoles([Parent] Mutation mutation, [Service]IMongoCollection<User> userCollection, AssignRoleInput input)
        {
            var user = await userManager.FindByIdAsync(input.UserId);
            user.UserRoles.Clear();
            var result = await userManager.AddToRolesAsync(user, input.Roles);
            if (result.Succeeded)
            {
                return true;
            }
            throw new Exception(result.Errors.First().Description);
        }
    }
}