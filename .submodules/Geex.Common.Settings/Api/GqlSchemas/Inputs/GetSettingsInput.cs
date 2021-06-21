using Geex.Common.Settings.Abstraction;

namespace Geex.Common.Settings.Api.GqlSchemas.Inputs
{
    public class GetSettingsInput
    {
        public SettingScopeEnumeration Scope { get; set; }
    }
}