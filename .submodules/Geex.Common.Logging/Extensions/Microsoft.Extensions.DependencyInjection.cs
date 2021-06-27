using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geex.Common.Gql;
using Geex.Common.Logging;

using HotChocolate.Execution.Configuration;
using HotChocolate.Execution.Instrumentation;
using HotChocolate.Execution.Options;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class Extension
    {
        public static IRequestExecutorBuilder AddGeexApolloTracing(
      this IRequestExecutorBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            builder.Services.TryAddTransient<ITimestampProvider, DefaultTimestampProvider>();
            return builder.ConfigureSchemaServices((Action<IServiceCollection>)(s => s.AddSingleton<IDiagnosticEventListener, GeexApolloTracingDiagnosticEventListener>(provider => new GeexApolloTracingDiagnosticEventListener(provider.GetApplicationService<ILogger<GeexApolloTracingDiagnosticEventListener>>(), provider.GetApplicationService<LoggingModuleOptions>(), provider.GetApplicationService<ITimestampProvider>()))));
        }
    }
}
