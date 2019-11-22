using System;
using System.Linq;
using System.Threading.Tasks;
using Geex.Core.UserManagement.Inputs;
using Geex.Shared.Roots;
using HotChocolate;
using IdentityModel;
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



        public async Task<bool> Register([Parent] Mutation mutation, [Service]IMongoCollection<User> userCollection, [Service]IPasswordHasher<User> passwordHasher, RegisterUserInput input)
        {
            await userCollection.InsertOneAsync(new User(input.PhoneOrEmail, input.Password, input.UserName, (u, x) => passwordHasher.HashPassword(u, x)));
            return true;
        }

        public async Task<bool> AssignRoles([Parent] Mutation mutation, [Service]IMongoCollection<User> userCollection, AssignRoleInput input)
        {
            var user = await userCollection.Find(x => x.Id == input.UserId).FirstOrDefaultAsync();
            user.Roles.Clear();
            foreach (var role in input.Roles)
            {
                user.Roles.Add(new Role(role));
            }
            return true;
        }
    }
}