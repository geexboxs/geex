using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.MongoDB.Mappers;
using Microsoft.Extensions.Hosting;

namespace Geex.Core
{
    public class IdentityServerSeedConfig
    {
        private IHostEnvironment _env;

        public IdentityServerSeedConfig(IHostEnvironment env)
        {
            _env = env;
            Clients = new List<Client>()
            {
                new Client()
                {
                    AllowedScopes = new List<string>(){
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        JwtClaimTypes.Role,
                        _env.ApplicationName },
                    RedirectUris = new List<string>(){ "*" },
                    PostLogoutRedirectUris = new List<string>(){ "*" },
                    AllowedCorsOrigins = new List<string>(){"*"},
                    ClientId = _env.ApplicationName,
                    ClientName = _env.ApplicationName,
                    ClientSecrets = new List<Secret>(){new Secret(_env.ApplicationName.Sha256())},
                    RequireConsent = false,
                    AllowAccessTokensViaBrowser = true,
                    AllowedGrantTypes = GrantTypes.Hybrid.Concat(GrantTypes.ResourceOwnerPassword).ToList(),
                    //FrontChannelLogoutUri = "https://localhost:44302/Home/FrontChannelLogout",
                    //BackChannelLogoutUri = "https://localhost:44302/Home/FrontChannelLogout",
                    AllowOfflineAccess = true,
                },
            };
            ApiResources = new List<ApiResource>()
            {
                new ApiResource()
                {
                    ApiSecrets = new List<Secret>(){new Secret(_env.ApplicationName) },
                    Name = _env.ApplicationName,
                    Scopes = new List<Scope>(){new Scope(_env.ApplicationName),new Scope(IdentityServerConstants.StandardScopes.Profile),new Scope(IdentityServerConstants.StandardScopes.OpenId) },
                }
            };
        }
        public List<ApiResource> ApiResources { get; set; }
        public List<Client> Clients { get; set; }

        public List<IdentityResource> IdentityResources { get; set; } = new List<IdentityResource>()
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
            new IdentityResources.Phone(),
            new IdentityResource()
            {
                Name = JwtClaimTypes.Role,
                DisplayName = "Your role",
                Emphasize = true,
                UserClaims = {JwtClaimTypes.Role},
            },
            new IdentityResource()
            {
                Name = "permissions",
                DisplayName = "Your permissions",
                Emphasize = true,
                UserClaims = {"permissions"},
            }
        };
    }
}
