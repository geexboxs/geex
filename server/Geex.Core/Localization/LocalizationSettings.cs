﻿using System.Text.Json;
using Geex.Core.Settings.Domain;
using JetBrains.Annotations;

namespace Geex.Core.Localization
{
    public class LocalizationSettings : SettingDefinition
    {
        public LocalizationSettings([NotNull] string name, [CanBeNull] object? defaultValue, [CanBeNull] string? description = null, bool isHiddenForClients = false) : base(name, defaultValue, description, isHiddenForClients)
        {
        }
        public static LocalizationSettings LocalizationLanguage { get; } = new(nameof(LocalizationLanguage), "en-US");
        public static LocalizationSettings LocalizationData { get; } = new(nameof(LocalizationData), "{\"en-US\":{\"common\":{\"test\":\"fuck\"}}}".ToObject<object>());
    }
}
