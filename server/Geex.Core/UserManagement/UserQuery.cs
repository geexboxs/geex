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
    public class UserQuery : QueryTypeExtension<UserQuery>
    {
        [GraphQLDescription("This field does ...")]
        public IQueryable<User> QueryUsers(
            [Service] DbContext dbContext)
        {
            return dbContext.Queryable<User>();
        }

        public async Task<IUserProfile> UserProfile(
            [Service] DbContext dbContext,
            string userIdentifier)
        {
            return await dbContext.Find<User>().MatchUserIdentifier(userIdentifier).ExecuteFirstAsync();
        }
    }
}
