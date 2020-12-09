using System.Collections;
using System.Collections.Generic;
using MongoDB.Bson;

namespace Geex.Core.Users.GqlSchemas.Inputs
{
    public class AddToOrgInput
    {
        public ObjectId UserId { get; set; }
        public List<string> Orgs { get; set; }
    }
}