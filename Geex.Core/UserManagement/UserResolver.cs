using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Autofac;

using Geex.Core.Authentication.GqlSchemas.Inputs;
using Geex.Core.Authentication.GqlSchemas.Types;
using Geex.Core.UserManagement.GqlSchemas.Inputs;
using Geex.Core.Users;
using Geex.Shared._ShouldMigrateToLib;
using Geex.Shared._ShouldMigrateToLib.Abstractions;
using Geex.Shared._ShouldMigrateToLib.Auth;
using Geex.Shared.Roots;

using HotChocolate;
using IdentityModel.Client;
using IdentityServer4.Services;
using MongoDB.Driver;
using MongoDB.Entities;

namespace Geex.Core.UserManagement
{
    //[GraphQLResolverOf(typeof(User))]
    //[GraphQLResolverOf(typeof(Query))]
    public class UserResolver
    {
        [GraphQLDescription("This field does ...")]
        public IQueryable<User> QueryUsers([Parent] Query query)
        {
            return DB.Collection<User>().AsQueryable();
        }
        public async Task<bool> Register([Parent] Mutation mutation,
            [Service] IComponentContext componentContext,
            RegisterUserInput input)
        {
            var user = new User(input.PhoneOrEmail, input.Password, input.UserName);
            await user.SaveAsync();
            return true;
        }

        public async Task<bool> AssignRoles([Parent] Mutation mutation, AssignRoleInput input)
        {
            var user = await DB.Collection<User>().FirstAsync(x => x.ID == input.UserId.ToString());
            await user.Roles.RemoveAsync(user.Roles.Select(x => x.ID));
            foreach (var role in input.Roles)
            {
                await user.Roles.AddAsync(new Role(role));
            }
            await user.SaveAsync();
            return true;
        }

        public async Task<TokenResponse> Authentication([Parent] Mutation mutation, IIdentityServerInteractionService identityService, AuthenticateInput input)
        {
            var user = await DB.Collection<User>().FirstOrDefaultAsync(x => x.ID == input.UserIdentifier || x.PhoneNumber == input.UserIdentifier || x.Email == input.UserIdentifier);
            if (user == default)
            {
                throw new UserFriendlyException("invalid username or password");
            }

            if (!user.CheckPassword(input.Password))
            {
                throw new UserFriendlyException("invalid username or password");
            }
            return new IdentityServer4.Services.DefaultTokenService().
        }
    }
}