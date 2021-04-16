
using System.Collections.Generic;

using Geex.Shared._ShouldMigrateToLib.Abstractions;

using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace Geex.Core.SystemSettings.Domain
{
    public class EnumerationSettingDefinitionProvider<T> : SettingDefinitionProvider where T : RmsSettingsBase
    {
        public override void Define(ISettingDefinitionContext context)
        {
            foreach (var rmsSettings in
                (typeof(T).GetProperty(nameof(RmsSettingsBase.List)).GetValue(null) as IEnumerable<T>))
            {
                context.Add(new SettingDefinition(rmsSettings.Value, rmsSettings.DefaultValue, new FixedLocalizableString(rmsSettings.Name), new FixedLocalizableString(rmsSettings.Description), rmsSettings.IsVisibleToClients));
            }
        }
    }
}