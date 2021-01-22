using System.Threading.Tasks;

using Autofac;

using Geex.Core.Authentication.Domain;
using Geex.Core.UserManagement.GqlSchemas.Inputs;
using Geex.Shared._ShouldMigrateToLib;
using Geex.Shared.Roots;

using HotChocolate;
using HotChocolate.Types;

using MongoDB.Entities;

namespace Geex.Core.UserManagement
{
    [ExtendObjectType(nameof(Mutation))]
    public class UserMutation : Mutation
    {
        public async Task<bool> Register([Parent] Mutation mutation,
            [Service] IComponentContext componentContext,
            RegisterUserInput input)
        {
            var user = new User(input.PhoneOrEmail, input.Password, input.UserName);
            await user.SaveAsync();
            return true;
        }
        public async Task<bool> AssignRoles([Parent] Mutation mutation,[ScopedService] DbContext dbContext, AssignRoleInput input)
        {
            var user = await dbContext.Find<User>().OneAsync(input.UserId.ToString());
            await user.SaveAsync();
            await user.AssignRoles(input.Roles);
            return true;
        }
    }
}