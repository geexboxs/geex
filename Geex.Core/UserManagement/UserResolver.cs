using System;
using System.Linq;
using System.Threading.Tasks;

using Autofac;

using Geex.Core.UserManagement.GqlSchemas.Inputs;
using Geex.Core.Users;
using Geex.Shared._ShouldMigrateToLib;
using Geex.Shared._ShouldMigrateToLib.Abstractions;
using Geex.Shared._ShouldMigrateToLib.Auth;
using Geex.Shared.Roots;

using HotChocolate;

using MongoDB.Driver;
using MongoDB.Entities;

namespace Geex.Core.UserManagement
{
    //[GraphQLResolverOf(typeof(User))]
    //[GraphQLResolverOf(typeof(Query))]
    public class UserResolver
    {
        [GraphQLDescription("This field does ...")]
        public IQueryable<User> QueryUsers([Parent] Query query, [Service] IMongoCollection<User> userCollection)
        {
            return userCollection.AsQueryable();
        }
        public async Task<bool> Register([Parent] Mutation mutation,
            [Service] IComponentContext componentContext,
            RegisterUserInput input)
        {
            var user = new User(input.PhoneOrEmail, input.Password, input.UserName);
            await user.SaveAsync();
            return true;
        }

        public async Task<bool> AssignRoles([Parent] Mutation mutation, [Service] IMongoCollection<User> userCollection, AssignRoleInput input)
        {
            var user = await userCollection.FirstAsync(x => x.ID == input.UserId.ToString());
            await user.Roles.RemoveAsync(user.Roles.Select(x=>x.ID));
            foreach (var role in input.Roles)
            {
                await user.Roles.AddAsync(new Role(role));
            }
            await user.SaveAsync();
            return true;
        }
    }
}