using System.Collections.Generic;
using HotChocolate;
using HotChocolate.Types;
using MongoDB.Bson;

namespace Geex.Core.UserManagement.GqlSchemas.Inputs
{
    public record AssignRoleInput
    {
        public ObjectId UserId { get; set; }
        public List<string> Roles { get; set; }
    }
}