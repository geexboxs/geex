using Geex.Shared._ShouldMigrateToLib.Abstractions;

namespace Geex.Core.Authorization
{
    public class AuthorizeTargetType : Enumeration<AuthorizeTargetType, string>
    {
        public static readonly AuthorizeTargetType Role = new AuthorizeTargetType(nameof(Role));
        public static readonly AuthorizeTargetType User = new AuthorizeTargetType(nameof(User));

        public AuthorizeTargetType(string value) : base(value)
        {
        }
    }
}