using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Geex.Core.Authentication.Domain;

using MongoDB.Entities;

namespace Geex.Core.Authentication.Utils
{
    public class UserCreationValidator : IUserCreationValidator
    {
        public DbContext DbContext { get; set; }

        public UserCreationValidator(DbContext dbContext)
        {
            this.DbContext = dbContext;
        }
    }
}
