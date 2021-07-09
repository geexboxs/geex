using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Autofac;

using Geex.Common.Gql.Roots;
using Geex.Core.UserManagement.GqlSchemas.Inputs;
using Geex.Core.Users;
using Geex.Shared._ShouldMigrateToLib.Auth;

using HotChocolate;
using HotChocolate.Types;

using MongoDB.Bson;
using MongoDB.Driver.Linq;
using MongoDB.Entities;


namespace Geex.Core.UserManagement
{
    public class RoleMutation : MutationTypeExtension<RoleMutation>
    {
        public async Task<bool> CreateRole(
            [Service] IComponentContext componentContext,
            CreateRoleInput input)
        {
            var role = new Role(input.RoleName);
            await role.SaveAsync();
            return true;
        }
    }
}
