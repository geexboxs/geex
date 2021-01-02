using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Autofac;

using Geex.Shared._ShouldMigrateToLib;

using HotChocolate;
using HotChocolate.Execution.Configuration;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Modularity;

namespace Geex.Shared
{
    public abstract class GraphQLModule<T> : GraphQLModule where T : GraphQLModule
    {

    }
    [DependsOn(typeof(FrameworkFixModule))]
    public class GraphQLModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            base.ConfigureServices(context);
            context.Services.GetSingletonInstance<IRequestExecutorBuilder>().AddModuleTypes(this.GetType());
        }

        public IRequestExecutorBuilder SchemaBuilder => this.ServiceConfigurationContext.Services.GetSingletonInstance<IRequestExecutorBuilder>();

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            base.OnApplicationInitialization(context);
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            base.OnApplicationShutdown(context);
        }

        public override void OnPostApplicationInitialization(ApplicationInitializationContext context)
        {
            base.OnPostApplicationInitialization(context);
        }

        public override void OnPreApplicationInitialization(ApplicationInitializationContext context)
        {
            base.OnPreApplicationInitialization(context);
        }

        public override void PostConfigureServices(ServiceConfigurationContext context)
        {
            base.PostConfigureServices(context);
        }

        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            base.PreConfigureServices(context);
        }

        public static HashSet<Assembly> KnownAssembly { get; } = new HashSet<Assembly>();
    }

    public abstract class GraphQLEntryModule<T> : GraphQLModule<T> where T : GraphQLModule
    {
    }
}
