using System.Collections.Generic;
using System.Linq;
using Geex.Core.User.Types.Inputs;
using Geex.Shared.Roots;
using HotChocolate;
using HotChocolate.Resolvers;

namespace Geex.Core.User
{
    [GraphQLResolverOf(typeof(User))]
    [GraphQLResolverOf(typeof(Query))]
    public class UserResolver
    {
        [GraphQLDescription("This field does ...")]
        public IQueryable<User> QueryUsers([Parent] Query query)
        {
            return new EnumerableQuery<User>(Users);
        }

        public static List<User> Users { get; set; } = new List<User>
            {
                new User
                {
                    Id = "1"
                },
                new User
                {
                    Id = "2"
                },
                new User
                {
                    Id = "3"
                },
                new User
                {
                    Id = "4"
                }
            };

        public bool Register([Parent] Mutation mutation, IResolverContext context)
        {
            var input = context.Argument<RegisterUserInputType.RegisterUserInput>("input");
            Users.Add(new User() { UserName = input.UserName });
            return true;
        }
    }
}