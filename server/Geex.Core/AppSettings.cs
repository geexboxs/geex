using Geex.Common.Settings;
using Geex.Common.Settings.Abstraction;

using JetBrains.Annotations;

namespace Geex.Core.Settings
{
    public class AppSettings : SettingDefinition
    {
        public enum AclMode
        {
            allOf,
            oneOf
        }

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

        public static AppSettings AppName { get; } = new(nameof(AppName), "Geex", new[] { SettingScopeEnumeration.Global });

        public static AppSettings AppMenu { get; } = new(nameof(AppMenu), new AlainMenuItem[]
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
        }, new[] { SettingScopeEnumeration.Global });

        public AppSettings([NotNull] string name, [NotNull] object? defaultValue,
            SettingScopeEnumeration[] validScopes = default,
            [CanBeNull] string? description = null, bool isHiddenForClients = false) : base("App" + name, defaultValue,
            validScopes, description, isHiddenForClients)
        {
        }
    }

    internal record AlainAclObject(string Role, string[] Ability, AppSettings.AclMode Mode, bool Except);
}