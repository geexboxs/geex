using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Geex.Common.Settings.Abstraction;
using Geex.Common.Settings.Core;
using Geex.Core.Settings;

using MongoDB.Entities;

namespace Geex.Core.Migrations
{
    public class _20210624162947_init : IMigration
    {
        internal record AlainMenuItem
        {
            public string i18n { get; init; }
            public bool group { get; init; }
            public bool hideInBreadcrumb { get; init; }
            public AlainMenuItem[] children { get; init; }
            public string icon { get; init; }
            public bool shortcutRoot { get; init; }
            public string link { get; init; }
            public int badge { get; init; }
            public AlainAclObject acl { get; init; }
            public bool shortcut { get; set; }
        }
    internal record AlainAclObject(string Role, string[] Ability, AppSettings.AclMode Mode, bool Except);
        public async Task UpgradeAsync(DbContext dbContext)
        {
            var nameSetting = new Setting(AppSettings.AppName, "Geex", SettingScopeEnumeration.Global);
            var menuSetting = new Setting(AppSettings.AppMenu, Enumerable.Empty<int>().ToJson(), SettingScopeEnumeration.Global);
            dbContext.AttachContextSession(new[] { nameSetting, menuSetting });
            await nameSetting.SaveAsync();
            await menuSetting.SaveAsync();
        }
    }
}
