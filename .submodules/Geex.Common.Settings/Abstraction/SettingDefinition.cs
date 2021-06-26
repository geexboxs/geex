using System;
using System.Runtime.Serialization;
using System.Text.Json;

using Geex.Common.Abstractions;
using JetBrains.Annotations;

namespace Geex.Common.Settings.Abstraction
{
    public class SettingDefinition : Enumeration<SettingDefinition, string>
    {
        public string Description { get; }
        public SettingScopeEnumeration[] ValidScopes { get; }
        public bool IsHiddenForClients { get; }

        public SettingDefinition(string name, SettingScopeEnumeration[] validScopes = default, string? description = null, bool isHiddenForClients = false) : base(name, name)
        {
            Description = description ?? name;
            ValidScopes = validScopes ?? new[] { SettingScopeEnumeration.Global, SettingScopeEnumeration.User, };
            IsHiddenForClients = isHiddenForClients;
        }
    }
}