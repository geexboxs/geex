using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommonServiceLocator;

using Geex.Core.Authentication.Domain;
using Geex.Core.Authentication.Utils;
using Geex.Core.UserManagement.Domain;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

using MongoDB.Entities;

namespace Geex.Core.Authentication.Migrations
{
    public class _20210705110119_init_admin : IMigration
    {
        public async Task UpgradeAsync(DbContext dbContext)
        {
            var user = new User(ServiceLocator.Current.GetInstance<IUserCreationValidator>(), ServiceLocator.Current.GetInstance<IPasswordHasher<User>>(), "admin@geex.com", "geex", "admin");
            dbContext.AttachContextSession(user);
            await user.SaveAsync();
        }
    }
}
