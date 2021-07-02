using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Geex.Common.Messaging.Api;
using Geex.Common.Messaging.Core;

using Geex.Common;
using Geex.Common.Abstractions;
using Geex.Common.Settings;

using Volo.Abp.Modularity;

namespace Geex.Common.Messaging
{
    [DependsOn(
        typeof(GeexCommonModule),
        typeof(CommonMessagingCoreModule),
        typeof(CommonMessagingApiModule)
        )]
    public class AppModule : GeexEntryModule<AppModule>
    {

    }
}
