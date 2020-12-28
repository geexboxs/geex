using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Geex.Shared._ShouldMigrateToLib;
using Geex.Shared._ShouldMigrateToLib.Abstractions;
using Geex.Shared._ShouldMigrateToLib.Auth;

using JetBrains.Annotations;

using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Geex.Core.Authentication.Domain
{
    public class UserToken : IdentityUserToken<string>
    {
        public new LoginProvider LoginProvider
        {
            get => LoginProvider.FromValue(base.LoginProvider);
            set => base.LoginProvider = value;
        }
        public UserToken(User user, LoginProvider loginProvider, UserTokenGenerateOptions options)
        {
            var (issuer, audience, expires, secretKey) = options;
            loginProvider ??= Domain.LoginProvider.Local;
            this.Name = user.Username;
            this.LoginProvider = loginProvider;
            this.UserId = user.Id;
            this.Value = new JwtSecurityTokenHandler().CreateEncodedJwt(issuer,
                audience,
                new ClaimsIdentity(new Claim[] { new JwtClaim(JwtClaimType.Sub, user.Id) }),
                default, DateTime.Now.Add(expires),
                DateTimeOffset.Now.LocalDateTime, new SigningCredentials(new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secretKey)
            ), SecurityAlgorithms.HmacSha256Signature));
        }
    }

    public class LoginProvider : Enumeration<LoginProvider, string>
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

    public record UserTokenGenerateOptions
    {
        public string Issuer;
        public string Audience;
        public TimeSpan Expires;
        public string SecretKey;

        public UserTokenGenerateOptions(string issuer, string audience, TimeSpan expires, string secretKey)
        {
            this.Issuer = issuer;
            this.Audience = audience;
            this.Expires = expires;
            this.SecretKey = secretKey;
        }

        public override bool Equals(object? obj)
        {
            return obj is UserTokenGenerateOptions other &&
                   Issuer == other.Issuer &&
                   Audience == other.Audience &&
                   Expires.Equals(other.Expires) &&
                   SecretKey == other.SecretKey;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Issuer, Audience, Expires, SecretKey);
        }

        public void Deconstruct(out string issuer, out string audience, out TimeSpan expires, out string secretKey)
        {
            issuer = this.Issuer;
            audience = this.Audience;
            expires = this.Expires;
            secretKey = this.SecretKey;
        }

        public static implicit operator (string issuer, string audience, TimeSpan expires, string secretKey)(UserTokenGenerateOptions value)
        {
            return (value.Issuer, value.Audience, value.Expires, value.SecretKey);
        }

        public static implicit operator UserTokenGenerateOptions((string issuer, string audience, TimeSpan expires, string secretKey) value)
        {
            return new UserTokenGenerateOptions(value.issuer, value.audience, value.expires, value.secretKey);
        }
    }
}
