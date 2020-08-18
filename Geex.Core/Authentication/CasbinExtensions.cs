using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using NetCasbin.Model;

using Volo.Abp.Domain.Repositories;

namespace Geex.Shared._ShouldMigrateToLib.Auth
{
    public static class CasbinExtensions
    {
        public static void AddCasbinAuthorization(this IServiceCollection services)
        {
            // Replace the default authorization policy provider with our own
            // custom provider which can return authorization policies for given
            // policy names (instead of using the default policy provider)
            services.AddSingleton(x => new CasbinMongoAdapter(() => x.GetService<IRepository<CasbinRule>>()));
            services.AddSingleton<Enforcer>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Permission", x =>
                {
                    x.AuthenticationSchemes.Add(CookieAuthenticationDefaults.AuthenticationScheme);
                    x.RequireAssertion(authContext =>
                    {
                        return authContext.User.HasClaim(p => p.Type == "permissions");
                    });
                });
            });
            //services.AddSingleton<IAuthorizationPolicyProvider, CasbinAuthorizationPolicyProvider>();

            //// As always, handlers must be provided for the requirements of the authorization policies
            //services.AddScoped<IAuthorizationHandler, CasbinAuthorizationHandler>();
        }
        public static Model LoadFromText(this Model model, string text)
        {
            model.LoadModelFromText(text);
            return model;
        }
    }
}