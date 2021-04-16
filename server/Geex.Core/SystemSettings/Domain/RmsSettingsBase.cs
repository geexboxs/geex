using Geex.Shared._ShouldMigrateToLib.Abstractions;

namespace Geex.Core.SystemSettings.Domain
{
    public abstract class RmsSettingsBase : Enumeration<RmsSettingsBase, string>
    {
        public const string Prefix = "Rms";
        //Add your own setting names here. Example:
        protected RmsSettingsBase(string displayName, string settingEnumString, bool isVisibleToClients) : base(displayName,
            $"{Prefix}.{settingEnumString}")
        {
            IsVisibleToClients = isVisibleToClients;
            Description = Name;
        }

        public string DefaultValue { get; } = null;
        public string Description { get; }
        public bool IsVisibleToClients { get; }
    }
}