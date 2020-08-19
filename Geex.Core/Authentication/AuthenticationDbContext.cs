using Geex.Shared._ShouldMigrateToLib.Auth;

using MongoDB.Driver;

using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Geex.Core.Authentication
{
    [ConnectionStringName("Geex")]
    public class AuthenticationDbContext : AbpMongoDbContext
    {
        public IMongoCollection<User> User { get; set; }
        public IMongoCollection<UserClaimRef> UserClaimRef { get; set; }
        public IMongoCollection<UserClaimRef> UserClaimRef { get; set; }
        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.Entity<User>(b =>
            {
                b.CollectionName = nameof(Shared._ShouldMigrateToLib.Auth.User); //Sets the collection name
            }); modelBuilder.Entity<UserClaimRef>(b =>
             {
                 b.CollectionName = nameof(UserClaimRef); //Sets the collection name
            }); modelBuilder.Entity<UserClaimRef>(b =>
             {
                 b.CollectionName = nameof(UserClaimRef); //Sets the collection name
            });
        }
    }
}