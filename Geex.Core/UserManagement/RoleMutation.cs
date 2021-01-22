using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Autofac;

using Geex.Core.UserManagement.GqlSchemas.Inputs;
using Geex.Core.Users;
using Geex.Shared._ShouldMigrateToLib.Abstractions;
using Geex.Shared._ShouldMigrateToLib.Auth;
using Geex.Shared.Roots;

using HotChocolate;
using HotChocolate.Types;

using MongoDB.Bson;
using MongoDB.Driver.Linq;
using MongoDB.Entities;


namespace Geex.Core.UserManagement
{
    [ExtendObjectType(nameof(Mutation))]
    public class RoleMutation : Mutation
    {
        public async Task<bool> CreateRole([Parent] Mutation mutation,
            [Service] IComponentContext componentContext,
            CreateRoleInput input)
        {
            var role = new Role(input.RoleName);
            await role.SaveAsync();
            return true;
        }
    }
}
