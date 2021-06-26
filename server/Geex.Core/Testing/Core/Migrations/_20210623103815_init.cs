using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geex.Core.Testing.Core.Aggregates.Tests;
using MongoDB.Entities;

namespace Geex.Core.Testing.Core.Migrations
{
    public class _20210623103815_init : IMigration
    {
        public async Task UpgradeAsync(DbContext dbContext)
        {
            var testEntity = new Test()
            {
                Name = "test",
                Data = "test"
            };
            dbContext.AttachContextSession(testEntity);
            await testEntity.SaveAsync();
        }
    }
}
