using Geex.Common.Settings.Abstraction;

namespace Geex.Core.Testing.Api
{
    public class TestingSettings : SettingDefinition
    {
        public TestingSettings(string name,
            SettingScopeEnumeration[] validScopes = default,
            string? description = null,
            bool isHiddenForClients = false) : base(nameof(Testing) + name, validScopes, description, isHiddenForClients)
        {
        }
        public static TestingSettings ModuleName { get; } = new(nameof(ModuleName), new[] { SettingScopeEnumeration.Global, });

    }
}
