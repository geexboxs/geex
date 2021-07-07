using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text.RegularExpressions;

using Geex.Common;
using Geex.Common.Abstraction;
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
        public static IServiceCollection AddStorage(this IServiceCollection builder)
        {
            var commonModuleOptions = builder.GetSingletonInstance<GeexCoreModuleOptions>();
            var mongoUrl = new MongoUrl(commonModuleOptions.ConnectionString) { };
            var mongoSettings = MongoClientSettings.FromUrl(mongoUrl);
            mongoSettings.ApplicationName = commonModuleOptions.AppName;
            DB.InitAsync(mongoUrl.DatabaseName, mongoSettings).Wait();
            builder.AddScoped<DbContext>(x => new DbContext(transactional: true));
            return builder;
        }

        public static void UseGeexGraphQL(this IApplicationBuilder app)
        {
            app.UseWebSockets();
            app.UseRouting()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapGraphQL();
                });
            app.UseVoyager("/graphql", "/voyager");
            app.UsePlayground("/graphql", "/playground");
        }
        public static IRequestExecutorBuilder AddModuleTypes<TModule>(this IRequestExecutorBuilder schemaBuilder)

        {
            return schemaBuilder
                .AddModuleTypes(typeof(TModule));
        }
        public static object? GetSingletonInstanceOrNull(this IServiceCollection services, Type type) => services.FirstOrDefault<ServiceDescriptor>((Func<ServiceDescriptor, bool>)(d => d.ServiceType == type))?.ImplementationInstance;
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
                var rootTypes = exportedTypes.Where(x => 
                                                         (x.BaseType == typeof(Query) ||
                                                          x.BaseType == typeof(Mutation) ||
                                                          x.BaseType == typeof(Subscription))).ToArray();
                //schemaBuilder.AddTypes(rootTypes);
                foreach (var rootType in rootTypes)
                {
                    schemaBuilder.AddTypeExtension(rootType);

                }
                var objectTypes = exportedTypes.Where(x => !x.IsAbstract && x.IsAssignableTo<IType>()).Where(x => !x.IsGenericType || (x.IsGenericType && x.GenericTypeArguments.Any())).ToList();
                schemaBuilder.AddTypes(objectTypes.ToArray());
                var classEnumTypes = exportedTypes.Where(x => !x.IsAbstract && x.IsClassEnum() && x.Name != nameof(Enumeration)).ToList();
                foreach (var classEnumType in classEnumTypes)
                {
                    schemaBuilder.BindRuntimeType(classEnumType, typeof(EnumerationType<,>).MakeGenericType(classEnumType, classEnumType.GetClassEnumValueType()));
                }
                foreach (var socketInterceptor in exportedTypes.Where(x => x.IsAssignableTo<ISocketSessionInterceptor>()).ToList())
                {
                    schemaBuilder.ConfigureSchemaServices(s => s.Add(ServiceDescriptor.Scoped(typeof(ISocketSessionInterceptor), socketInterceptor)));
                }

                foreach (var requestInterceptor in exportedTypes.Where(x => x.IsAssignableTo<IHttpRequestInterceptor>()).ToList())
                {
                    schemaBuilder.ConfigureSchemaServices(s => s.Add(ServiceDescriptor.Scoped(typeof(IHttpRequestInterceptor), requestInterceptor)));
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

        public static IEnumerable<T> GetSingletonInstancesOrNull<T>(this IServiceCollection services) => services.Where<ServiceDescriptor>((Func<ServiceDescriptor, bool>)(d => d.ServiceType == typeof(T)))?.Select(x => x.ImplementationInstance).Cast<T>();

        public static IEnumerable<T> GetSingletonInstances<T>(this IServiceCollection services) => services.GetSingletonInstancesOrNull<T>() ?? throw new InvalidOperationException("Could not find singleton service: " + typeof(T).AssemblyQualifiedName);
    }
}
