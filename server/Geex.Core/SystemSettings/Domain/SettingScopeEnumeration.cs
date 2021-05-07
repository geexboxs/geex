using Geex.Shared._ShouldMigrateToLib.Abstractions;

namespace Geex.Core.SystemSettings.Domain
{
    public class SettingScopeEnumeration : Enumeration<SettingScopeEnumeration, string>
    {

        public SettingScopeEnumeration(string name, string value) : base(name, value)
        {
        }
        /// <summary>
        /// 全局运行时
        /// </summary>
        public static SettingScopeEnumeration Global { get; } = new(nameof(Global), nameof(Global));
        /// <summary>
        /// 用户级
        /// </summary>
        public static SettingScopeEnumeration User { get; } = new(nameof(User), nameof(User));
    }
}
