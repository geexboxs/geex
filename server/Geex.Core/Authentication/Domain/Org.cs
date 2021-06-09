using Geex.Core.Authentication.Domain;
using MongoDB.Entities;
using Entity = MongoDB.Entities.Entity;

namespace Geex.Shared._ShouldMigrateToLib.Auth
{
    public class Org : Entity
    {
        public string Code { get; set; }
        [OwnerSide]
        public Many<User> Users { get; set; }
        public Many<Org> SubOrgs { get; set; }

        public Org()
        {
            this.InitManyToMany(x => x.Users, user => user.Orgs);
        }
    }
}