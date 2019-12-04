using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Geex.Shared._ShouldMigrateToLib.Auth
{
    public class GeexCookieAuthenticationEvents : CookieAuthenticationEvents
    {

        public GeexCookieAuthenticationEvents()
        {
            // Get the database from registered DI services.
        }

        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            var userPrincipal = context.Principal;
        }

    }
}
