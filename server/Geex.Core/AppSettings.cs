using Geex.Common.Settings;
using Geex.Common.Settings.Abstraction;

using JetBrains.Annotations;

namespace Geex.Core.Settings
{
    public class AppSettings : SettingDefinition
    {
        public enum AclMode
        {
            allOf,
            oneOf
        }

        

        public static AppSettings AppName { get; } = new(nameof(AppName),  new[] { SettingScopeEnumeration.Global });

        public static AppSettings AppMenu { get; } = new(nameof(AppMenu), new[] { SettingScopeEnumeration.Global });

        public AppSettings([NotNull] string name,
            SettingScopeEnumeration[] validScopes = default,
            [CanBeNull] string? description = null, bool isHiddenForClients = false) : base("App" + name,
            validScopes, description, isHiddenForClients)
        {
        }
    }

}