using Geex.Shared._ShouldMigrateToLib.Abstractions;

namespace Geex.Shared._ShouldMigrateToLib
{
    public class GeexClaimType : Enumeration<GeexClaimType, string>
    {
        public const string _Sub = "sub";
        public static GeexClaimType Sub { get; } = new GeexClaimType(nameof(Sub), _Sub);
        public const string _UserName = "name";
        public static GeexClaimType UserName { get; } = new GeexClaimType(nameof(UserName), _UserName);
        public const string _Provider = "login_provider";
        public static GeexClaimType Provider { get; } = new GeexClaimType(nameof(Provider), _Provider);
        public const string _Expires = "expires";
        public static GeexClaimType Expires { get; } = new GeexClaimType(nameof(Expires), _Expires);
        public GeexClaimType(string name, string value) : base(name, value)
        {
        }
    }
}