using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommonServiceLocator;

using Geex.Core;
using Geex.Shared._ShouldMigrateToLib.Auth;

using Humanizer;


using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MongoDB.Driver;
using MongoDB.Entities;

namespace Geex.Data.Migrations
{
    public class _0001_init : IMigration
    {
        public async Task UpgradeAsync()
        {
        }
    }
}
