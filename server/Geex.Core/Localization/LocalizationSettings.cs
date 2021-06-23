using System.Text.Json;

using Geex.Common.Settings;
using Geex.Common.Settings.Abstraction;

using JetBrains.Annotations;

namespace Geex.Core.Localization
{
    public class LocalizationSettings : SettingDefinition
    {
        public LocalizationSettings([NotNull] string name, [CanBeNull] object? defaultValue, [CanBeNull] string? description = null, bool isHiddenForClients = false) : base(nameof(Localization) + name, defaultValue, new[] { SettingScopeEnumeration.Global, }, description, isHiddenForClients)
        {
        }
        public static LocalizationSettings Language { get; } = new(nameof(Language), "en-US");
        public static LocalizationSettings Data { get; } = new(nameof(Data), "{\"en-US\":{\"common\":{\"test\":\"fuck\"}}}".ToObject<object>());
    }
}
