using Geex.Shared._ShouldMigrateToLib.Auth;
using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Geex.Core.Users
{
    [ConnectionStringName("Geex")]
    public class UserDbContext:AbpMongoDbContext
    {
        public IMongoCollection<User> User { get; set; }

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {

            base.CreateModel(modelBuilder);

            modelBuilder.Entity<User>(b =>
            {
                b.CollectionName = nameof(Shared._ShouldMigrateToLib.Auth.User); //Sets the collection name
            });
        }
    }
}