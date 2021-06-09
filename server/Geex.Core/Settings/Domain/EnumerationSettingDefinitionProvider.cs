﻿using System.Text.Json;
using Geex.Common.Abstractions;

namespace Geex.Core.Settings.Domain
{
    public abstract class SettingDefinition : Enumeration<SettingDefinition, string>
    {
        public string? DefaultValue { get; }
        public string Description { get; }
        public bool IsHiddenForClients { get; }

        protected SettingDefinition(string name, object? defaultValue, string? description = null, bool isHiddenForClients = false) : base(name, name)
        {
            DefaultValue = defaultValue is string str ? str : defaultValue?.ToJson();
            Description = description ?? name;
            IsHiddenForClients = isHiddenForClients;
        }
    }
}