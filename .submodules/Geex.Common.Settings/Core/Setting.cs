using System.Diagnostics;

using Geex.Common.Abstractions;
using Geex.Common.Gql.Types;
using Geex.Common.Settings.Abstraction;
using Geex.Common.Settings.Api.Aggregates.Settings;

using HotChocolate;

namespace Geex.Common.Settings.Core
{
    [DebuggerDisplay("{Name}")]
    public class Setting : Entity, ISetting
    {
        public SettingScopeEnumeration Scope { get; private set; }
        public string? ScopedKey { get; private set; }
        public string? Value { get; private set; }
        public SettingDefinition Name { get; private set; }
        public Setting(SettingDefinition name, string value, SettingScopeEnumeration scope, string? scopedKey = default)
        {
            Name = name;
            Value = value;
            Scope = scope;
            ScopedKey = scopedKey;
        }

        public void SetValue(string? value)
        {
            this.Value = value;
        }
    }
}