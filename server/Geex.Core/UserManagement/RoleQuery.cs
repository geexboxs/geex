using System.Linq;
using System.Threading.Tasks;

using Autofac;

using Geex.Common.Gql.Roots;
using Geex.Core.UserManagement.GqlSchemas.Inputs;
using Geex.Core.Users;

using HotChocolate;
using HotChocolate.Types;

using MongoDB.Driver;
using MongoDB.Entities;

namespace Geex.Core.UserManagement
{
    public class RoleQuery : QueryTypeExtension<RoleQuery>
    {
        public async Task<IQueryable<Role>> QueryRoles(
            [Service] DbContext dbContext,
            CreateRoleInput input)
        {
            return dbContext.Queryable<Role>();

        }
    }
}