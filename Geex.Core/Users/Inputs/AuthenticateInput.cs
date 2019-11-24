namespace Geex.Core.Users.Inputs
{
    public class AuthenticateInput
    {
        public string UserIdentifier { get; set; }
        public string Password { get; set; }
        public string RedirectUri { get; set; }
    }
}