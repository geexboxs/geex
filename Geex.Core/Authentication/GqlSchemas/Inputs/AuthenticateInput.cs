using HotChocolate.Types;

namespace Geex.Core.Authentication.GqlSchemas.Inputs
{
    public record AuthenticateInput
    {
        public string UserIdentifier { get; set; }
        public string Password { get; set; }
    }
}