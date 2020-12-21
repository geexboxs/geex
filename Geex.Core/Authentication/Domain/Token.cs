using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geex.Shared._ShouldMigrateToLib.Abstractions;
using Geex.Shared._ShouldMigrateToLib.Auth;
using Microsoft.AspNetCore.Identity;

namespace Geex.Core.Authentication.Domain
{
    public class Token : IdentityUserToken<string>
    {
        public Token(User user, LoginProvider loginProvider = LoginProvider.Local)
        {
            this.Name = user.Username;
            this.LoginProvider = user.Username;
            this.Name = user.Username;
            this.Name = user.Username;
        }
    }
    
    public class LoginProvider : Enumeration<string>
    {
        public static readonly LoginProvider Local = new LoginProvider(LoginProvider._Local);
        public const string _Local = "1";
        [DisplayName(LoginProvider.Local)]
        public string Type { get; set; }
    }
}
