using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Geex.Common.Abstractions;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Geex.Common.Logging
{
    public class LoggingModule : GeexModule<LoggingModule>
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.GetSingletonInstance<IRequestExecutorBuilder>()
                .AddGeexApolloTracing();
            base.ConfigureServices(context);
        }
    }
}
