using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Geex.Core.SystemSettings.Domain;

using JetBrains.Annotations;

namespace Geex.Core.SystemSettings
{
    public class ModuleSettings : SettingDefinition
    {
        public static ModuleSettings MaxSettingCount { get; } = new(nameof(MaxSettingCount), 3);

        public ModuleSettings([NotNull] string name, [NotNull] object? defaultValue, [CanBeNull] string? description = null, bool isHiddenForClients = false) : base(name, defaultValue, description, isHiddenForClients)
        {
        }
    }
}
