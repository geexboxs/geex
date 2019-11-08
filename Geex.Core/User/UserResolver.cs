using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Geex.Core.User.Inputs;
using Geex.Core.User.Types.Inputs;
using Geex.Shared.Roots;
using HotChocolate;
using HotChocolate.Resolvers;
using Microex.All.IdentityServer;
using Microsoft.EntityFrameworkCore;

namespace Geex.Core.User
{
    [GraphQLResolverOf(typeof(User))]
    [GraphQLResolverOf(typeof(Query))]
    public class UserResolver
    {
        [GraphQLDescription("This field does ...")]
        public IQueryable<User> QueryUsers([Parent] Query query, [Service]MicroexUserManager<User> userManager)
        {
            return userManager.Users;
        }



        public async Task<bool> Register([Parent] Mutation mutation, [Service]MicroexUserManager<User> userManager, RegisterUserInput input)
        {
            await userManager.CreateAsync(new User(input.PhoneOrEmail, input.UserName), input.Password);
            return true;
        }
    }
}