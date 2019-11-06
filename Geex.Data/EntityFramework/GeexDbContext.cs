using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Geex.Data.EntityFramework
{
    public class GeexDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GeexDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
