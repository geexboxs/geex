using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Geex.Common.Gql.Roots;
using Geex.Core.Authentication.Domain;
using Geex.Core.Authentication.GqlSchemas.Inputs;
using Geex.Core.Authentication.GqlSchemas.Types;
using Geex.Core.UserManagement.Domain;
using Geex.Core.Users;
using Geex.Shared._ShouldMigrateToLib.Auth;
using HotChocolate;
using HotChocolate.Types;

using MongoDB.Driver;
using MongoDB.Entities;

namespace Geex.Core.UserManagement
{
    [ExtendObjectType(nameof(Query))]
    public class UserQuery : Query
    {
        [GraphQLDescription("This field does ...")]
        public IQueryable<User> QueryUsers([Parent] Query query,
            [Service] DbContext dbContext)
        {
            return dbContext.Queryable<User>();
        }

        public async Task<IUserProfile> UserProfile([Parent] Query query,
            [Service] DbContext dbContext,
            string userIdentifier)
        {
            return await dbContext.Find<User>().MatchUserIdentifier(userIdentifier).ExecuteFirstAsync();
        }
    }
}
