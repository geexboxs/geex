using HotChocolate.Types;

namespace Geex.Core.Authentication.GqlSchemas.Inputs
{
    public class AuthenticateInput
    {
        public string UserIdentifier { get; set; }
        public string Password { get; set; }
    }
}