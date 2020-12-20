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
using Geex.Core.Authentication.GqlSchemas.Inputs;
using Geex.Shared._ShouldMigrateToLib.Auth;
using Microsoft.Extensions.Configuration;

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
            IHttpContextAccessor httpContextAccessor = componentContext.Resolve<IHttpContextAccessor>();
            IConfiguration configuration = componentContext.Resolve<IConfiguration>();
            //if (httpContextAccessor.HttpContext.Request.Headers.TryGetValue("Authentication", out var authValue))
            //{
            //    await httpContextAccessor.HttpContext.AuthenticateAsync();
            //}
            var client = new HttpClient() { BaseAddress = new Uri($"http://{configuration.GetAppHostAddress()}") };
            var tokenResponse = await client.RequestTokenAsync(new TokenRequest()
            {
                Address = "http://localhost:8000/connect/token",
                ClientId = configuration.GetAppName(),
                ClientSecret = configuration.GetAppName(),
                GrantType = GrantType.ResourceOwnerPassword,
                Parameters = new Dictionary<string, string>() {
                    { "username", input.UserIdentifier },
                    { "password", input.Password },
                    { "scope", $"{configuration.GetAppName()}" }
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
