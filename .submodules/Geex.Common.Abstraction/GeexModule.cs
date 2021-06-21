using System;
using System.Collections.Generic;
using System.Reflection;

using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Volo.Abp;
using Volo.Abp.Modularity;

namespace Geex.Common.Abstractions
{
    public abstract class GeexModule<T> : GeexModule where T : GeexModule
    {
        public IConfiguration Configuration { get; private set; }
        
        public virtual void ConfigureModuleOptions<TOptions>(Action<TOptions> optionsAction) where TOptions : GeexModuleOption<T>
        {
            base.Configure<TOptions>(optionsAction);
        }
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            Configuration = context.Services.GetConfiguration();
            context.Services.Add(new ServiceDescriptor(typeof(GeexModule), this));
            context.Services.Add(new ServiceDescriptor(this.GetType(), this));
            base.PreConfigureServices(context);
        }


        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            base.ConfigureServices(context);
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            foreach (var hasStartupInitialize in context.ServiceProvider.GetServices<IHasStartupInitialize>())
            {
                hasStartupInitialize.Initialize().Wait();
            }
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

    }
    public class GeexModule : AbpModule
    {
        public static HashSet<Assembly> KnownAssembly { get; } = new HashSet<Assembly>();
    }

    public abstract class GeexEntryModule<T> : GeexModule<T> where T : GeexModule
    {
        public IRequestExecutorBuilder SchemaBuilder => this.ServiceConfigurationContext.Services.GetSingletonInstance<IRequestExecutorBuilder>();
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.GetSingletonInstance<IRequestExecutorBuilder>().AddModuleTypes(this.GetType());
            base.ConfigureServices(context);
        }
    }
}
