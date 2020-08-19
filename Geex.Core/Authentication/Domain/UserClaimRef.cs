using System;
using MongoDB.Bson;
using Volo.Abp.Domain.Entities;

namespace Geex.Shared._ShouldMigrateToLib.Auth
{
    public class UserClaimRef : IEntity
    {
        public object[] GetKeys()
        {
            throw new NotImplementedException();
        }

        public ObjectId UserId { get; set; }
    }
}