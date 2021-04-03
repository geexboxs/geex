using Geex.Shared._ShouldMigrateToLib.Abstractions;

namespace Geex.Core.Authorization
{
    public class AppPermission : Enumeration<AppPermission, string>
    {
        public AppPermission(string value) : base(value)
        {
        }

        public static readonly AppPermission AssignRole = new AppPermission(nameof(AssignRole));
    }
}