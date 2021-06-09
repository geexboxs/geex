using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Geex.Common.Abstractions;
using Geex.Shared._ShouldMigrateToLib;
using Geex.Shared._ShouldMigrateToLib.Auth;

using JetBrains.Annotations;

using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Geex.Core.Authentication.Domain
{
    public class UserToken : IdentityUserToken<string>
    {
        public new LoginProvider? LoginProvider
        {
            get => (LoginProvider)base.LoginProvider;
            set => base.LoginProvider = value ?? throw new ArgumentNullException(nameof(value));
        }

        public UserToken(User user, LoginProvider provider, UserTokenGenerateOptions options)
        {
            UserId = user.Id;
            Name = user.UserName;
            LoginProvider = provider;
            Value = new JwtSecurityTokenHandler().CreateEncodedJwt(new Utils.GeexSecurityTokenDescriptor(user, provider, options));
        }
    }

    public class LoginProvider : Enumeration<LoginProvider, string>
    {
        public static LoginProvider Local { get; } = new LoginProvider(LoginProvider._Local);
        public const string _Local = nameof(Local);
        public LoginProvider([NotNull] string name, string value) : base(name, value)
        {
        }

        public LoginProvider(string value) : base(value)
        {
        }
    }

    public record UserTokenGenerateOptions
    {
        public string Issuer;
        public string Audience;
        public TimeSpan Expires;
        public string SecretKey;

        public UserTokenGenerateOptions(string issuer, string audience, string secretKey, TimeSpan expires)
        {
            this.Issuer = issuer;
            this.Audience = audience;
            this.Expires = expires;
            this.SecretKey = secretKey;
        }
    }
}
