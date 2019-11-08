using IdentityServer4.EntityFramework.Options;
using Microex.All.IdentityServer;
using Microsoft.EntityFrameworkCore;

namespace Geex.Data
{
    public class GeexDbContext : IdentityServerDbContext<Core.User.User>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GeexDbContext).Assembly);
        }

        public GeexDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
