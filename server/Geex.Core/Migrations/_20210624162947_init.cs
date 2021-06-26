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
            var menuSetting = new Setting(AppSettings.AppMenu, new AlainMenuItem[]
        {
            new()
            {
                i18n = "menu.mine",
                @group = true,
                hideInBreadcrumb = true,
                children = new AlainMenuItem[]
                {
                    new()
                    {
                        i18n = "menu.shortcut",
                        icon = "anticon-rocket",
                        shortcutRoot = true
                    }
                }
            },
            new()
            {
                i18n = "menu.main",
                @group = true,
                hideInBreadcrumb = true,
                children = new AlainMenuItem[]
                {
                    new()
                    {
                        i18n = "menu.dashboard",
                        icon = "anticon-dashboard",
                        children = new AlainMenuItem[]
                        {
                            new()
                            {
                                link = "/dashboard/main",
                                icon = "anticon-dashboard",
                                i18n = "menu.dashboard.main"
                            },
                            new()
                            {
                                link = "/dashboard/analysis",
                                icon = "anticon-dashboard",
                                i18n = "menu.dashboard.analysis"
                            },
                            new()
                            {
                                link = "/dashboard/monitor",
                                icon = "anticon-dashboard",
                                i18n = "menu.dashboard.monitor"
                            },
                            new()
                            {
                                link = "/dashboard/workplace",
                                icon = "anticon-dashboard",
                                shortcut = true,
                                i18n = "menu.dashboard.workplace"
                            }
                        }
                    },
                }
            },
            new()
            {
                i18n = "menu.administration",
                @group = true,
                hideInBreadcrumb = true,
                children = new AlainMenuItem[]
                {
                    new()
                    {
                        i18n = "menu.userManagement",
                        icon = "anticon-dashboard",
                        children = new AlainMenuItem[]
                        {
                            new()
                            {
                                link = "/userManagement/organization",
                                icon = "anticon-dashboard",
                                i18n = "menu.userManagement.organization"
                            },
                            new()
                            {
                                link = "/userManagement/role",
                                icon = "anticon-dashboard",
                                i18n = "menu.userManagement.role"
                            },
                            new()
                            {
                                link = "/userManagement/user",
                                icon = "anticon-dashboard",
                                i18n = "menu.userManagement.user"
                            },
                        }
                    },
                    new()
                    {
                        i18n = "menu.settings",
                        icon = "anticon-dashboard",
                        children = new AlainMenuItem[]
                        {
                            new()
                            {
                                link = "/userManagement/user",
                                icon = "anticon-dashboard",
                                i18n = "menu.settings.user"
                            },
                            new()
                            {
                                link = "/userManagement/global",
                                icon = "anticon-dashboard",
                                i18n = "menu.settings.global"
                            },
                        }
                    },
                }
            },
        }.ToJson(), SettingScopeEnumeration.Global);
            dbContext.AttachContextSession(new[] { nameSetting, menuSetting });
            await nameSetting.SaveAsync();
            await menuSetting.SaveAsync();
        }
    }
}
