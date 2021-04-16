using System;

using Geex.Shared._ShouldMigrateToLib.Abstractions;

using MongoDB.Bson;
using MongoDB.Entities;

using Volo.Abp.Domain.Entities;

namespace Geex.Shared._ShouldMigrateToLib.Auth
{
    public class UserClaim
    {
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

        public UserClaim(string claimType, string claimValue)
        {
            this.ClaimType = claimType;
            ClaimValue = claimValue;
        }

    }
}