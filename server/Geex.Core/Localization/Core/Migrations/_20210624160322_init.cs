using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Geex.Common.Settings.Abstraction;
using Geex.Common.Settings.Core;
using MongoDB.Entities;

namespace Geex.Core.Localization.Core.Migrations
{
    public class _20210624160322_init : IMigration
    {
        public async Task UpgradeAsync(DbContext dbContext)
        {
            var languageSetting = new Setting(LocalizationSettings.Language, "en-US", SettingScopeEnumeration.Global);
            var dataSetting = new Setting(LocalizationSettings.Data, "{\"en-US\":{\"common\":{\"test\":\"fuck\"}}}".ToObject<object>().ToJson(), SettingScopeEnumeration.Global);
            dbContext.AttachContextSession(new[] { languageSetting, dataSetting });
            await languageSetting.SaveAsync();
            await dataSetting.SaveAsync();
        }
    }
}
