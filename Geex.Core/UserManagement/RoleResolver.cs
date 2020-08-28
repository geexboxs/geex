using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autofac;

using Geex.Core.Users;
using Geex.Shared._ShouldMigrateToLib.Abstractions;
using Geex.Shared._ShouldMigrateToLib.Auth;
using Geex.Shared.Roots;

using HotChocolate;
using HotChocolate.Types;
using MongoDB.Bson;

using Volo.Abp.Domain.Repositories;

namespace Geex.Core.UserManagement
{
    [GraphQLResolverOf(typeof(Role))]
    [GraphQLResolverOf(typeof(Query))]
    public class RoleResolver
    {
        public async Task<bool> CreateRole([Parent] Mutation mutation,
            [Service] IComponentContext componentContext,
            CreateRoleInput input)
        {
            var role = new Role(input.RoleName);
            var repository = componentContext.Resolve<IRepository<Role>>();
            await repository.InsertAsync(role);
            return true;
        }
    }

    public class CreateRoleInput
    {
        public string RoleName { get; set; }
    }
}
