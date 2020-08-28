using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Geex.Core.UserManagement.GqlSchemas.Inputs;
using Geex.Core.Users;
using Geex.Shared._ShouldMigrateToLib.Abstractions;
using Geex.Shared._ShouldMigrateToLib.Auth;
using Geex.Shared.Roots;
using HotChocolate;
using Volo.Abp.Domain.Repositories.MongoDB;

namespace Geex.Core.UserManagement
{
    //[GraphQLResolverOf(typeof(AppUser))]
    //[GraphQLResolverOf(typeof(Query))]
    public class UserResolver
    {
        [GraphQLDescription("This field does ...")]
        public IQueryable<AppUser> QueryUsers([Parent] Query query, [Service]IMongoDbRepository<AppUser> userCollection)
        {
            return userCollection;
        }
        public async Task<bool> Register([Parent] Mutation mutation,
            [Service]IComponentContext componentContext,
            RegisterUserInput input)
        {
            var user = new AppUser(input.PhoneOrEmail, input.Password, input.UserName);
            await user.As<IActiveRecord<AppUser>>().SaveAsync();
            return true;
        }

        public async Task<bool> AssignRoles([Parent] Mutation mutation, AssignRoleInput input)
        {
            var user = await IActiveRecord<AppUser>.StaticRepository.GetAsync(x=>x.Id == input.UserId);
            user.Roles.Clear();
            foreach (var role in input.Roles)
            {
                user.Roles.Add(new Role(role));
            }
            await user.As<IActiveRecord<AppUser>>().SaveAsync();
            return true;
        }
    }
}