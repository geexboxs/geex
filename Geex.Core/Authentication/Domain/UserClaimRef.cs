using System;

using Geex.Shared._ShouldMigrateToLib.Abstractions;

using MongoDB.Bson;

using Volo.Abp.Domain.Entities;

namespace Geex.Shared._ShouldMigrateToLib.Auth
{
    public class UserClaimRef : IEntity
    {
        public object[] GetKeys()
        {
            return new object[] { UserId, ClaimType };
        }

        public ObjectId UserId { get; set; }
        public ClaimType ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }

    public class ClaimType : ConstValue<string>
    {
    }
}