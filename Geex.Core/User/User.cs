using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microex.All.Common;
using Microex.All.IdentityServer.Identity;
using Microsoft.AspNetCore.Identity;

namespace Geex.Core.User
{
    public class User : GeexUser
    {
        [Obsolete("This ctor should not be used in user code.")]
        public User()
        {

        }
        public User(string phoneOrEmail, string userName = null)
        {
            if (phoneOrEmail.IsValidEmail())
            {
                this.Email = phoneOrEmail;
            }
            else
            {
                this.PhoneNumber = phoneOrEmail;
            }
            this.UserName = userName ?? phoneOrEmail;
        }

        
    }
}
