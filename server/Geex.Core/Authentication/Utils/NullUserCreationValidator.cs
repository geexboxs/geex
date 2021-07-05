using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geex.Core.Authentication.Domain;
using Geex.Core.UserManagement.Domain;
using MongoDB.Entities;

namespace Geex.Core.Authentication.Utils
{
    public class NullUserCreationValidator : IUserCreationValidator
    {
        public DbContext DbContext { get; set; }
        public void Check(User user)
        {
            return;
        }
    }
}
