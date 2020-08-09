using Autofac;

using Geex.Core.Users;
using Geex.Shared.Roots;

using HotChocolate;
using HotChocolate.Types;

using IdentityModel.Client;

using IdentityServer4.Models;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;

using MongoDB.Driver;

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Geex.Core.Authentication
{
    [GraphQLResolverOf(typeof(StringType))]
    [GraphQLResolverOf(typeof(Query))]
    public class AuthenticationResolver
    {
        public async Task<TokenResponse> Authenticate([Parent] Mutation mutation,
            [Service] IComponentContext componentContext,

            AuthenticateInput input)
        {
            IMongoCollection<AppUser> userCollection = componentContext.Resolve<IMongoCollection<AppUser>>();
            IHttpContextAccessor httpContextAccessor = componentContext.Resolve<IHttpContextAccessor>();
            IPasswordHasher<AppUser> passwordHasher = componentContext.Resolve<IPasswordHasher<AppUser>>();
            IHostEnvironment env = componentContext.Resolve<IHostEnvironment>();
            if (httpContextAccessor.HttpContext.Request.Headers.TryGetValue("Authentication", out var authValue))
            {
                await httpContextAccessor.HttpContext.AuthenticateAsync();
            }
            var client = new HttpClient() { BaseAddress = new Uri($"http://{Environment.GetEnvironmentVariable("HOST_NAME")}") };
            var tokenResponse = await client.RequestTokenAsync(new TokenRequest()
            {
                Address = "http://localhost:8000/connect/token",
                ClientId = env.ApplicationName,
                ClientSecret = env.ApplicationName,
                GrantType = GrantType.ResourceOwnerPassword,
                Parameters = new Dictionary<string, string>() {
                    { "username", input.UserIdentifier },
                    { "password", input.Password },
                    { "scope",$"{env.ApplicationName}" }
                }
            });
            if (tokenResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                return tokenResponse;
            }
            throw tokenResponse.Exception;
        }
    }
}
