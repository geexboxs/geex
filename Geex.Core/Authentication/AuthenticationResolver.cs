using Autofac;

using Geex.Core.Users;
using Geex.Shared.Roots;

using HotChocolate;
using HotChocolate.Types;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;

using MongoDB.Driver;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Geex.Core.Authentication.Domain;
using Geex.Core.Authentication.GqlSchemas.Inputs;
using Geex.Shared._ShouldMigrateToLib.Abstractions;
using Geex.Shared._ShouldMigrateToLib.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Entities;

namespace Geex.Core.Authentication
{
    [GraphQLResolverOf(typeof(StringType))]
    [GraphQLResolverOf(typeof(Query))]
    public class AuthenticationResolver
    {
        public async Task<IdentityUserToken<string>> Authenticate([Parent] Mutation mutation,
            [Service] IComponentContext componentContext,
            AuthenticateInput input)
        {
            IConfiguration configuration = componentContext.Resolve<IConfiguration>();
            var user = await User.FindAsync(input.UserIdentifier, CancellationToken.None);
            return new UserToken(user, LoginProvider.Local, componentContext.Resolve<GeexTokenOptions>());
            //return new UserToken(user, LoginProvider.Local, default);
        }
    }

    public class GeexTokenOptions
    {
        public GeexTokenOptions(string secretKey, TimeSpan expires)
        {
            SecretKey = secretKey;
            Expires = expires;
        }

        public string SecretKey { get; }
        public TimeSpan Expires { get; }
    }
    public class LoginProvider : Enumeration<LoginProvider, string>
    {
        public const string _Local = nameof(Local);
        public static LoginProvider Local { get; } = new LoginProvider(LoginProvider._Local);

        public LoginProvider(string value) : base(value)
        {
        }

    }


    public class UserToken : IdentityUserToken<string>
    {
        public new LoginProvider? LoginProvider
        {
            get => (LoginProvider)base.LoginProvider;
            set => base.LoginProvider = value ?? throw new ArgumentNullException(nameof(value));
        }

        public UserToken(User user, LoginProvider provider, GeexTokenOptions options)
        {
            UserId = user.ID;
            Name = user.UserName;
            LoginProvider = provider;
            Value = new JwtSecurityTokenHandler().CreateEncodedJwt(new GeexSecurityTokenDescriptor(user, provider, options));
        }
    }

    public class GeexSecurityTokenDescriptor : SecurityTokenDescriptor
    {
        public GeexSecurityTokenDescriptor(User user, LoginProvider provider, GeexTokenOptions options)
        {
            this.Audience = "*";
            this.SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey)), SecurityAlgorithms.HmacSha256Signature);
            Expires = DateTime.Now.Add(options.Expires);
            IssuedAt = DateTime.Now;
            Subject = new ClaimsIdentity(new Claim[]
            {
                new GeexClaim(GeexClaimType.Sub, user.ID),
                new GeexClaim(GeexClaimType.UserName, user.UserName),
                new GeexClaim(GeexClaimType.Provider, provider),
            });
        }
    }

    public class GeexClaim : Claim
    {
        protected GeexClaim(Claim other) : base(other)
        {
        }

        public GeexClaim(GeexClaimType type, string value) : base(type, value)
        {
        }
    }

    public class GeexClaimType : Enumeration<GeexClaimType, string>
    {
        public const string _Sub = "sub";
        public static GeexClaimType Sub { get; } = new GeexClaimType(nameof(Sub), _Sub);
        public const string _UserName = "name";
        public static GeexClaimType UserName { get; } = new GeexClaimType(nameof(UserName), _UserName);
        public const string _Provider = "name";
        public static GeexClaimType Provider { get; } = new GeexClaimType(nameof(Provider), _Provider);
        public GeexClaimType(string name, string value) : base(name, value)
        {
        }
    }
}
