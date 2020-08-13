namespace Geex.Core.Users.GqlSchemas.Inputs
{
    public class RegisterUserInput
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PhoneOrEmail { get; set; }
    }
}