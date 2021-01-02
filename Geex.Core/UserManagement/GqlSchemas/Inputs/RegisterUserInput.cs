namespace Geex.Core.UserManagement.GqlSchemas.Inputs
{
    public record RegisterUserInput
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PhoneOrEmail { get; set; }
    }
}