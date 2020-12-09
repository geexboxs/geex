using System;

using Geex.Shared._ShouldMigrateToLib.Abstractions;

using MongoDB.Bson;
using MongoDB.Entities;
using Volo.Abp.Domain.Entities;

namespace Geex.Shared._ShouldMigrateToLib.Auth
{
    public class UserClaim
    {
        public ClaimType ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }

    public class ClaimType : ConstValue<string>
    {
    }
}