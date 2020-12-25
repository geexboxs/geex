using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geex.Shared._ShouldMigrateToLib.Abstractions;
using Geex.Shared._ShouldMigrateToLib.Auth;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;

namespace Geex.Core.Authentication.Domain
{
    public class Token : IdentityUserToken<string>
    {
        public Token(User user, LoginProvider loginProvider)
        {
            loginProvider ??= Domain.LoginProvider.Local;
            this.Name = user.Username;
            this.LoginProvider = loginProvider;
            this.Name = user.Username;
            this.Name = user.Username;
        }
    }
    
    public class LoginProvider : Enumeration<LoginProvider,string>
    {
        public static readonly LoginProvider Local = new LoginProvider(LoginProvider._Local);
        public const string _Local = nameof(Local);
        public LoginProvider([NotNull] string name, string value) : base(name, value)
        {
        }

        public LoginProvider(string value) : base(value)
        {
        }
    }
}
