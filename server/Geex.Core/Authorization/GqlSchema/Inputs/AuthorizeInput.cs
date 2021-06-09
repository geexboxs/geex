using System.Collections.Generic;
using HotChocolate;
using HotChocolate.Types;
using MongoDB.Bson;

namespace Geex.Core.Authorization
{
    public record AuthorizeInput
    {
        public AuthorizeTargetType AuthorizeTargetType { get; set; }
        public List<AppPermission> AllowedPermissions { get; set; }
        public ObjectId TargetId { get; set; }
    }
}