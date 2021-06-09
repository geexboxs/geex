using Geex.Common.Abstractions;

namespace Geex.Core.Settings.Domain
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
        /// 对当前用户生效的配置
        /// </summary>
        public static SettingScopeEnumeration Effective { get; } = new(nameof(Effective), nameof(Effective));
        /// <summary>
        /// 用户级
        /// </summary>
        public static SettingScopeEnumeration User { get; } = new(nameof(User), nameof(User));
    }
}
