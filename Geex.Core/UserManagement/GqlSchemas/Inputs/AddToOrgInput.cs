using System.Collections;
using System.Collections.Generic;
using Geex.Shared.Types;
using HotChocolate;
using HotChocolate.Types;

using MongoDB.Bson;

namespace Geex.Core.Users.GqlSchemas.Inputs
{
    public record AddToOrgInput
    {
        public ObjectId UserId { get; set; }
        public List<string> Orgs { get; set; }
    }
}