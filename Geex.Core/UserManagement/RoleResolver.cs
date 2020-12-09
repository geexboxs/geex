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
using MongoDB.Driver;
using MongoDB.Entities;


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
            await role.SaveAsync();
            var repository = componentContext.Resolve<IMongoCollection<Role>>();
            return true;
        }
    }

    public class CreateRoleInput
    {
        public string RoleName { get; set; }
    }
}
