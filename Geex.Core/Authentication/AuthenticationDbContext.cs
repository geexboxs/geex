using Geex.Shared._ShouldMigrateToLib.Auth;

using MongoDB.Driver;

using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Geex.Core.Authentication
{
    [ConnectionStringName("Geex")]
    public class AuthenticationDbContext : AbpMongoDbContext
    {
        public IMongoCollection<AppUser> User { get; set; }
        public IMongoCollection<UserClaimRef> UserClaimRef { get; set; }
        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.Entity<AppUser>(b =>
            {
                b.CollectionName = nameof(AppUser); //Sets the collection name
            }); modelBuilder.Entity<UserClaimRef>(b =>
             {
                 b.CollectionName = nameof(UserClaimRef); //Sets the collection name
            });
        }
    }
}