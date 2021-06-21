using System.Diagnostics;
using Geex.Common.Abstractions;
using Geex.Common.Gql.Types;
using Geex.Common.Settings.Abstraction;
using HotChocolate;

namespace Geex.Common.Settings.Core
{
    [DebuggerDisplay("{Name}")]
    public class Setting : IHasId
    {
        [GraphQLType(typeof(EnumerationType<SettingScopeEnumeration, string>))]
        public SettingScopeEnumeration Scope { get; }
        public string? ScopedKey { get; }
        public string? Value { get; }
        public SettingDefinition Name { get; }
        string IHasId.Id => $"{this.Scope}:{this.Name}{(this.ScopedKey == default ? "" : $":{this.ScopedKey}")}";

        public Setting(SettingDefinition name, string value, SettingScopeEnumeration scope, string? scopedKey = default)
        {
            Name = name;
            Value = value;
            Scope = scope;
            ScopedKey = scopedKey;
        }
    }
}