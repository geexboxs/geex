using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text.RegularExpressions;

using Geex.Common;
using Geex.Common.Abstractions;
using Geex.Common.Gql;
using Geex.Common.Gql.Interceptors;
using Geex.Common.Gql.Roots;
using Geex.Common.Gql.Types;

using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Voyager;
using HotChocolate.Execution.Configuration;
using HotChocolate.Execution.Instrumentation;
using HotChocolate.Execution.Options;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using HotChocolate.Types.Pagination;
using HotChocolate.Utilities;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Entities;

using Volo.Abp.Modularity;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class Extension
    {

        public static void UseGeexGraphQL(this IApplicationBuilder app)
        {
            app.UseRouting()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapGraphQL();
                });
            app.UseVoyager("/graphql", "/voyager");
            app.UsePlayground();
        }

        public static IRequestExecutorBuilder AddModuleTypes<TModule>(this IRequestExecutorBuilder schemaBuilder)
        {
            return schemaBuilder
                .AddModuleTypes(typeof(TModule));
        }
        public static object? GetSingletonInstanceOrNull(this IServiceCollection services, Type type) => services.FirstOrDefault<ServiceDescriptor>((Func<ServiceDescriptor, bool>) (d => d.ServiceType == type))?.ImplementationInstance;
        public static IRequestExecutorBuilder AddModuleTypes(this IRequestExecutorBuilder schemaBuilder, Type gqlModuleType)
        {
            if (GeexModule.KnownModuleAssembly.AddIfNotContains(gqlModuleType.Assembly))
            {
                var dependedModuleTypes = gqlModuleType.GetCustomAttribute<DependsOnAttribute>()?.DependedTypes;
                if (dependedModuleTypes?.Any() == true)
                {
                    foreach (var dependedModuleType in dependedModuleTypes)
                    {
                        schemaBuilder.AddModuleTypes(dependedModuleType);
                    }
                }
                var exportedTypes = gqlModuleType.Assembly.GetExportedTypes();
                var rootTypes = exportedTypes.Where(x => !x.IsAbstract &&
                                                              (AbpTypeExtensions.IsAssignableTo<Query>(x) ||
                                                               AbpTypeExtensions.IsAssignableTo<Mutation>(x) ||
                                                               AbpTypeExtensions.IsAssignableTo<Subscription>(x))).ToArray();
                schemaBuilder.AddTypes(rootTypes);
                var objectTypes = exportedTypes.Where(x => !x.IsAbstract && AbpTypeExtensions.IsAssignableTo<IType>(x)).ToList();
                schemaBuilder.AddTypes(objectTypes.ToArray());
                var classEnumTypes = exportedTypes.Where(x => !x.IsAbstract && x.IsClassEnum()).ToList();
                foreach (var classEnumType in classEnumTypes)
                {
                    schemaBuilder.BindRuntimeType(classEnumType, typeof(EnumerationType<,>).MakeGenericType(classEnumType, classEnumType.GetClassEnumValueType()));
                }
            }
            return schemaBuilder;
        }

        public static bool IsValidEmail(this string str)
        {
            return new Regex(@"\w[-\w.+]*@([A-Za-z0-9][-A-Za-z0-9]+\.)+[A-Za-z]{2,14}").IsMatch(str);
        }

        public static bool IsValidPhoneNumber(this string str)
        {
            return new Regex(@"\d{11}").IsMatch(str);
        }



        public static bool IsClassEnum(this Type type)
        {
            if (type.IsValueType)
            {
                return false;
            }

            return AbpTypeExtensions.IsAssignableTo<IEnumeration>(type);
        }

        public static IRequestExecutorBuilder AddGeexApolloTracing(
      this IRequestExecutorBuilder builder,
      TracingPreference tracingPreference = TracingPreference.OnDemand,
      ITimestampProvider? timestampProvider = null)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            return tracingPreference == TracingPreference.Never ? builder : builder.ConfigureSchemaServices((Action<IServiceCollection>)(s => s.AddSingleton<IDiagnosticEventListener>((Func<IServiceProvider, IDiagnosticEventListener>)(sp => (IDiagnosticEventListener)new GeexApolloTracingDiagnosticEventListener(sp.GetApplicationService<ILogger<GeexApolloTracingDiagnosticEventListener>>(), tracingPreference, timestampProvider ?? sp.GetService<ITimestampProvider>())))));
        }

        public static IEnumerable<T> GetSingletonInstancesOrNull<T>(this IServiceCollection services) => services.Where<ServiceDescriptor>((Func<ServiceDescriptor, bool>)(d => d.ServiceType == typeof(T)))?.Select(x => x.ImplementationInstance).Cast<T>();

        public static IEnumerable<T> GetSingletonInstances<T>(this IServiceCollection services) => services.GetSingletonInstancesOrNull<T>() ?? throw new InvalidOperationException("Could not find singleton service: " + typeof(T).AssemblyQualifiedName);

    }
}
