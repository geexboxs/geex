using Geex.Shared._ShouldMigrateToLib.Abstractions;

namespace Geex.Core.Authorization
{
    public class AuthorizeTargetType : Enumeration<AuthorizeTargetType, string>
    {
        public static AuthorizeTargetType Role { get; set; }
        public static AuthorizeTargetType User { get; set; }

        public AuthorizeTargetType(string value) : base(value)
        {
        }
    }
}