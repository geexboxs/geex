using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Geex.Common.Settings.Abstraction;

using JetBrains.Annotations;

namespace Geex.Core.Testing.Api
{
    public class TestingSettings : SettingDefinition
    {
        public TestingSettings(string name,
            object? defaultValue,
            SettingScopeEnumeration[] validScopes = default,
            string? description = null,
            bool isHiddenForClients = false) : base(nameof(Testing) + name, defaultValue, validScopes, description, isHiddenForClients)
        {
        }
        public static TestingSettings ModuleName { get; } = new(nameof(ModuleName), "Testing");

    }
}
