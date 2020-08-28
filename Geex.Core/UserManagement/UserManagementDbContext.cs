using Geex.Core.Users;
using Geex.Shared._ShouldMigrateToLib.Auth;
using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Geex.Core.UserManagement
{
    [ConnectionStringName("Geex")]
    public class UserManagementDbContext:AbpMongoDbContext
    {
        public IMongoCollection<AppUser> User { get; set; }

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {

            base.CreateModel(modelBuilder);

            modelBuilder.Entity<Role>(b =>
            {
                b.CollectionName = nameof(Role); //Sets the collection name
            });
        }
    }
}