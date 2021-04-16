using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using Autofac;

using CommonServiceLocator;

using Geex.Shared._ShouldMigrateToLib;
using Geex.Shared._ShouldMigrateToLib.Abstractions;
using Geex.Shared._ShouldMigrateToLib.Json;
using Geex.Shared.Roots;
using Geex.Shared.Types;

using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Voyager;
using HotChocolate.Execution;
using HotChocolate.Execution.Configuration;
using HotChocolate.Execution.Instrumentation;
using HotChocolate.Execution.Options;
using HotChocolate.Execution.Serialization;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using HotChocolate.Types.Pagination;
using HotChocolate.Utilities;

using ImpromptuInterface;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DiagnosticAdapter;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.Extensions.Options;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Entities;

using NUglify.Helpers;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Uow;

using IConvention = HotChocolate.Types.Descriptors.IConvention;

namespace Geex.Shared
{
    public static class Extensions
    {
        public static IHostBuilder ConfigureGeexServer<T>(this IHostBuilder hostBuilder) where T : GraphQLEntryModule<T>
        {
            var containerBuilder = new ContainerBuilder();
            return hostBuilder.ConfigureServices((_, services) =>
                {
                    services.AddObjectAccessor(containerBuilder);
                    services.AddGeexGraphQL<T>();
                })
                .UseServiceProviderFactory(new AbpAutofacServiceProviderFactory(containerBuilder));
        }

        public static void AddGeexGraphQL<T>(this IServiceCollection services) where T : GraphQLEntryModule<T>
        {
            services.AddStorage();
            var schemaBuilder = services.AddGraphQLServer();
            schemaBuilder.AddConvention<ITypeInspector>(typeof(GeexTypeConvention))
                .AddFiltering()
            .AddSorting()
            .AddProjections()
            .SetPagingOptions(new PagingOptions()
            {
                DefaultPageSize = 10,
                IncludeTotalCount = true,
                MaxPageSize = 1000
            })
            //.AddHttpRequestInterceptor(((context, executor, builder, token) =>
            //{
            //    context.Response.OnCompleted(() => context.RequestServices.GetService<DbContext>().CommitAsync(token));
            //    return default;
            //}))
            .AddHttpRequestInterceptor<UnitOfWorkInterceptor>()
            //.ConfigureOnRequestExecutorCreatedAsync(((provider, executor, cancellationToken) =>
            //{
            //    return new ValueTask(provider.GetService<DbContext>().CommitAsync(cancellationToken));
            //}))
            .AddGeexTypes()
            .AddErrorFilter<UserFriendlyErrorFilter>()
            //.AddErrorFilter(x =>
            //{
            //    if (x.Exception is UserFriendlyException)
            //    {
            //        x.RemoveException();
            //    }
            //    else
            //    {

            //    }
            //    return x;
            //})
            .OnSchemaError((ctx, err) =>
            {
                throw new Exception("schema error", err);
            })
            //.UseExceptions()
            .AddAuthorization()
                .AddApolloTracing();
            //schemaBuilder.ConfigureSchemaServices(x=>x.AddApplication<T>());
            services.AddSingleton(schemaBuilder);
            services.AddApplication<T>();
        }

        private static IRequestExecutorBuilder AddGeexTypes(this IRequestExecutorBuilder services)
        {
            return services.AddQueryType<Query>()
                            .AddMutationType<Mutation>()
                            .AddSubscriptionType<Subscription>()
                            .BindRuntimeType<ObjectId, ObjectIdType>();
        }

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
                .AddTypes(typeof(TModule).Assembly.GetExportedTypes().Where(x => x.Namespace != null && x.Namespace.Contains($"{typeof(TModule).Namespace}.GqlSchemas")).ToArray());
        }

        public static IRequestExecutorBuilder AddModuleTypes(this IRequestExecutorBuilder schemaBuilder, Type gqlModuleType)
        {
            if (GraphQLModule.KnownAssembly.AddIfNotContains(gqlModuleType.Assembly))
            {
                var exportedTypes = gqlModuleType.Assembly.GetExportedTypes();
                var rootTypes = exportedTypes.Where(x => !x.IsAbstract &&
                                                              (AbpTypeExtensions.IsAssignableTo<Query>(x) ||
                                                               AbpTypeExtensions.IsAssignableTo<Mutation>(x) ||
                                                               AbpTypeExtensions.IsAssignableTo<Subscription>(x))).ToArray();
                schemaBuilder.AddTypes(rootTypes);
                var objectTypes = exportedTypes.Where(x => !x.IsAbstract && AbpTypeExtensions.IsAssignableTo<IType>(x)).ToArray();
                schemaBuilder.AddTypes(objectTypes);
                var classEnumTypes = exportedTypes.Where(x => !x.IsAbstract && x.BaseType.Name.StartsWith("Enumeration")).ToArray();
                foreach (var classEnumType in classEnumTypes)
                {
                    if (classEnumType.GenericTypeArguments.Length == 1)
                    {
                        schemaBuilder.BindRuntimeType(classEnumType, typeof(EnumerationType<,>).MakeGenericType(classEnumType.BaseType.GenericTypeArguments));
                    }
                    else
                    {
                        schemaBuilder.BindRuntimeType(classEnumType, typeof(EnumerationType<,>).MakeGenericType(classEnumType.BaseType.GenericTypeArguments));
                    }
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

        public static List<TFieldType> GetFieldsOfType<TFieldType>(this Type type)
        {
            return type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(p => type.IsAssignableFrom(p.FieldType))
                .Select(pi => (TFieldType)pi.GetValue(null))
                .ToList();
        }

        public static IServiceCollection AddStorage(this IServiceCollection builder, string connectionStringName = "GeexMongo")
        {
            var configuration = builder.GetConfiguration();
            var mongoUrl = new MongoUrl(configuration.GetConnectionString("GeexMongo")) { };
            var mongoSettings = MongoClientSettings.FromUrl(mongoUrl);
            mongoSettings.ApplicationName = configuration.GetAppName();
            DB.InitAsync(mongoUrl.DatabaseName, mongoSettings).Wait();
            builder.AddScoped<DbContext>(x => new DbContext(transactional: true));
            return builder;
        }

        public static IRequestExecutorBuilder AddApolloTracing(
      this IRequestExecutorBuilder builder,
      TracingPreference tracingPreference = TracingPreference.OnDemand,
      ITimestampProvider? timestampProvider = null)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            return tracingPreference == TracingPreference.Never ? builder : builder.ConfigureSchemaServices((Action<IServiceCollection>)(s => s.AddSingleton<IDiagnosticEventListener>((Func<IServiceProvider, IDiagnosticEventListener>)(sp => (IDiagnosticEventListener)new GeexApolloTracingDiagnosticEventListener(sp.GetApplicationService<ILogger<GeexApolloTracingDiagnosticEventListener>>(), tracingPreference, timestampProvider ?? sp.GetService<ITimestampProvider>())))));
        }
    }

    public class UserFriendlyErrorFilter : IErrorFilter
    {

        public UserFriendlyErrorFilter()
        {
            LoggerProvider = ServiceLocator.Current.GetInstance<ILoggerProvider>();

        }

        public ILoggerProvider LoggerProvider { get; }

        public IError OnError(IError error)
        {
            if (error.Exception is UserFriendlyException)
            {
                error.RemoveException();
            }

            if (error.Exception != default)
            {
                if (error.Exception?.TargetSite?.DeclaringType != default)
                {
                    LoggerProvider.CreateLogger(error.Exception.TargetSite.DeclaringType.FullName).LogException(error.Exception);
                }
                else
                {
                    LoggerProvider.CreateLogger("Null").LogException(error.Exception);
                }
            }
            return error;
        }
    }
}
