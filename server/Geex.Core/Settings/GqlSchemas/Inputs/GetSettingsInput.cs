using Geex.Core.Settings.Domain;

namespace Geex.Core.Settings.GqlSchemas.Inputs
{
    public class GetSettingsInput
    {
        public SettingScopeEnumeration Scope { get; set; }
    }
}