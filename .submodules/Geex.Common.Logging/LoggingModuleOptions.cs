using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Geex.Common.Abstractions;
using HotChocolate.Execution.Options;

namespace Geex.Common.Logging
{
    public class LoggingModuleOptions : IGeexModuleOption<LoggingModule>
    {
        public TracingPreference TracingPreference { get; set; } = TracingPreference.OnDemand;
    }
}
