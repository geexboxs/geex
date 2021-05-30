using Geex.Core.Settings.Domain;
using Geex.Shared._ShouldMigrateToLib.Json;
using JetBrains.Annotations;

namespace Geex.Core.Settings
{
    public class AppSettings : SettingDefinition
    {
        public static AppSettings AppName { get; } = new(nameof(AppName), "Geex");
        public static AppSettings AppMenu { get; } = new(nameof(AppMenu), "[{\"text\":\"主导航\",\"i18n\":\"menu.main\",\"group\":true,\"hideInBreadcrumb\":true,\"children\":[{\"text\":\"仪表盘\",\"i18n\":\"menu.dashboard\",\"icon\":\"anticon-dashboard\",\"children\":[{\"text\":\"仪表盘V1\",\"link\":\"/dashboard/v1\",\"i18n\":\"menu.dashboard.v1\"},{\"text\":\"分析页\",\"link\":\"/dashboard/analysis\",\"i18n\":\"menu.dashboard.analysis\"},{\"text\":\"监控页\",\"link\":\"/dashboard/monitor\",\"i18n\":\"menu.dashboard.monitor\"},{\"text\":\"工作台\",\"link\":\"/dashboard/workplace\",\"i18n\":\"menu.dashboard.workplace\"}]},{\"text\":\"快捷菜单\",\"i18n\":\"menu.shortcut\",\"icon\":\"anticon-rocket\",\"shortcutRoot\":true,\"children\":[]},{\"text\":\"小部件\",\"i18n\":\"menu.widgets\",\"link\":\"/widgets\",\"icon\":\"anticon-appstore\",\"badge\":2}]},{\"text\":\"Alain\",\"i18n\":\"menu.alain\",\"group\":true,\"hideInBreadcrumb\":true,\"children\":[{\"text\":\"样式\",\"i18n\":\"menu.style\",\"icon\":\"anticon-info\",\"children\":[{\"text\":\"Typography\",\"link\":\"/style/typography\",\"i18n\":\"menu.style.typography\",\"shortcut\":true},{\"text\":\"Grid Masonry\",\"link\":\"/style/gridmasonry\",\"i18n\":\"menu.style.gridmasonry\"},{\"text\":\"Colors\",\"link\":\"/style/colors\",\"i18n\":\"menu.style.colors\"}]},{\"text\":\"Delon\",\"i18n\":\"menu.delon\",\"icon\":\"anticon-bulb\",\"children\":[{\"text\":\"Dynamic Form\",\"link\":\"/delon/form\",\"i18n\":\"menu.delon.form\"},{\"text\":\"Simple Table\",\"link\":\"/delon/st\",\"i18n\":\"menu.delon.table\"},{\"text\":\"Util\",\"link\":\"/delon/util\",\"i18n\":\"menu.delon.util\",\"acl\":\"role-a\"},{\"text\":\"Print\",\"link\":\"/delon/print\",\"i18n\":\"menu.delon.print\",\"acl\":\"role-b\"},{\"text\":\"QR\",\"link\":\"/delon/qr\",\"i18n\":\"menu.delon.qr\"},{\"text\":\"ACL\",\"link\":\"/delon/acl\",\"i18n\":\"menu.delon.acl\"},{\"text\":\"Route Guard\",\"link\":\"/delon/guard\",\"i18n\":\"menu.delon.guard\"},{\"text\":\"Cache\",\"link\":\"/delon/cache\",\"i18n\":\"menu.delon.cache\"},{\"text\":\"Down File\",\"link\":\"/delon/downfile\",\"i18n\":\"menu.delon.downfile\"},{\"text\":\"Xlsx\",\"link\":\"/delon/xlsx\",\"i18n\":\"menu.delon.xlsx\"},{\"text\":\"Zip\",\"link\":\"/delon/zip\",\"i18n\":\"menu.delon.zip\"}]}]}]".ToObject<dynamic>());

        public AppSettings([NotNull] string name, [NotNull] object? defaultValue, [CanBeNull] string? description = null, bool isHiddenForClients = false) : base(name, defaultValue, description, isHiddenForClients)
        {
        }
    }
}
