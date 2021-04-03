using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Geex.Core.UserManagement.GqlSchemas.Inputs;
using Geex.Core.Users;
using Geex.Shared.Roots;
using HotChocolate;
using HotChocolate.Types;
using MongoDB.Driver;
using MongoDB.Entities;

namespace Geex.Core.UserManagement
{
    [ExtendObjectType(nameof(Query))]
    public class RoleQuery : Query
    {
        public async Task<IQueryable<Role>> QueryRoles([Parent] Query query,
            [Service] DbContext dbContext,
            CreateRoleInput input)
        {
            return dbContext.Queryable<Role>();

        }
    }
}