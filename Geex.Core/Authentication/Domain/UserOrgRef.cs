using Geex.Shared._ShouldMigrateToLib.Abstractions;
using MongoDB.Bson;
using Volo.Abp.Domain.Entities;

namespace Geex.Shared._ShouldMigrateToLib.Auth
{
    public class UserOrgRef : IActiveRecord<UserOrgRef>
    {
        public object[] GetKeys()
        {
            return new object[] { UserId, OrgId };
        }

        public ObjectId UserId { get; set; }
        public ObjectId OrgId { get; set; }
    }
}