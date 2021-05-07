using Geex.Core.SystemSettings.Domain;
using Geex.Shared._ShouldMigrateToLib;
using Geex.Shared.Types;

using HotChocolate;

namespace Geex.Core.SystemSettings
{
    public class Setting : IHasId
    {
        [GraphQLType(typeof(EnumerationType<SettingScopeEnumeration, string>))]
        public SettingScopeEnumeration Scope { get; }
        public string? ScopedKey { get; }
        public string Value { get; set; }
        public string Name { get; }
        string IHasId.Id => $"{this.Scope}:{this.Name}{(this.ScopedKey == default ? "" : $":{this.ScopedKey}")}";

        public Setting(string name, string value, SettingScopeEnumeration scope, string? scopedKey = default)
        {
            Name = name;
            Value = value;
            Scope = scope;
            ScopedKey = scopedKey;
        }
    }
}