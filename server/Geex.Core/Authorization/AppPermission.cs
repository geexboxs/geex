using Geex.Common.Abstractions;

namespace Geex.Core.Authorization
{
    public class AppPermission : Enumeration<AppPermission, string>
    {
        public AppPermission(string value) : base(value)
        {
        }

        public static AppPermission AssignRole { get; } = new AppPermission(nameof(AssignRole));
    }
}