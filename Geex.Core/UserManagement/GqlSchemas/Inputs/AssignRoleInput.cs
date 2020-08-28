using System.Collections.Generic;
using MongoDB.Bson;

namespace Geex.Core.UserManagement.GqlSchemas.Inputs
{
    public class AssignRoleInput
    {
        public ObjectId UserId { get; set; }
        public List<string> Roles { get; set; }
    }
}