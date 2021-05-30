using Geex.Shared._ShouldMigrateToLib.Abstractions;
using Geex.Shared._ShouldMigrateToLib.Json;

namespace Geex.Core.Settings.Domain
{
    public abstract class SettingDefinition : Enumeration<SettingDefinition, string>
    {
        public string? DefaultValue { get; }
        public string Description { get; }
        public bool IsHiddenForClients { get; }

        protected SettingDefinition(string name, object? defaultValue, string? description = null, bool isHiddenForClients = false) : base(name, name)
        {
            DefaultValue = defaultValue?.ToJson();
            Description = description ?? name;
            IsHiddenForClients = isHiddenForClients;
        }
    }
}