using Geex.Shared._ShouldMigrateToLib.Abstractions;

namespace Geex.Core.Authorization
{
    public class AppPermission : Enumeration<AppPermission, string>
    {
        public AppPermission(string value) : base(value)
        {
        }

        public const string _AssignRole = nameof(AssignRole);
        public static AppPermission AssignRole { get; } = new(_AssignRole);
    }
}