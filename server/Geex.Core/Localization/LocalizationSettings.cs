using System.Text.Json;

using Geex.Common.Settings;
using Geex.Common.Settings.Abstraction;

using JetBrains.Annotations;

namespace Geex.Core.Localization
{
    public class LocalizationSettings : SettingDefinition
    {
        public LocalizationSettings([NotNull] string name, SettingScopeEnumeration[] validScopes,
            [CanBeNull] string? description = null, bool isHiddenForClients = false) : base(nameof(Localization) + name, validScopes, description, isHiddenForClients)
        {
        }
        public static LocalizationSettings Language { get; } = new(nameof(Language), new[] { SettingScopeEnumeration.Global, SettingScopeEnumeration.User, });
        public static LocalizationSettings Data { get; } = new(nameof(Data), new[] { SettingScopeEnumeration.Global, });
    }
}
