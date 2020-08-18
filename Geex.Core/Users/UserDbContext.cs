using Geex.Shared._ShouldMigrateToLib.Auth;
using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Geex.Core.Users
{
    [ConnectionStringName("Geex")]
    public class UserDbContext:AbpMongoDbContext
    {
        public IMongoCollection<AppUser> User { get; set; }

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {

            base.CreateModel(modelBuilder);

            modelBuilder.Entity<AppUser>(b =>
            {
                b.CollectionName = nameof(AppUser); //Sets the collection name
            });
        }
    }
}