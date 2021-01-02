using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Autofac;

using Geex.Shared._ShouldMigrateToLib;
using Geex.Shared._ShouldMigrateToLib.Abstractions;
using Geex.Shared.Roots;
using Geex.Shared.Types;

using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Voyager;
using HotChocolate.Execution;
using HotChocolate.Execution.Configuration;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using HotChocolate.Types.Pagination;
using HotChocolate.Utilities;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

using Volo.Abp.Autofac;

namespace Geex.Shared
{
    public static class Extensions
    {
        public static IHostBuilder ConfigureGeexServer<T>(this IHostBuilder hostBuilder, string connectionStringName = "Geex") where T : GraphQLEntryModule<T>
        {
            var containerBuilder = new ContainerBuilder();
            return hostBuilder.ConfigureServices((_, services) =>
                {
                    services.AddObjectAccessor(containerBuilder);
                    services.AddGeexGraphQL<T>(connectionStringName);
                })
                .UseServiceProviderFactory(new AbpAutofacServiceProviderFactory(containerBuilder));
        }

        public static void AddGeexGraphQL<T>(this IServiceCollection services, string connectionStringName = "Geex") where T : GraphQLEntryModule<T>
        {
            services.AddStorage(connectionStringName);

            var schemaBuilder = services.AddGraphQLServer()
                .AddFiltering()
                .AddSorting()
                .AddProjections()
                .SetPagingOptions(new PagingOptions()
                {
                    DefaultPageSize = 10,
                    IncludeTotalCount = true,
                    MaxPageSize = 1000
                })
                .AddQueryType<Query>()
                .AddMutationType<Mutation>()
                .AddSubscriptionType<Subscription>()
                .BindRuntimeType<ObjectId, ObjectIdType>()
                .AddErrorFilter(x =>
                {
                    Console.WriteLine(x);
                    return x;
                })
                .OnSchemaError((ctx, err) =>
                {
                    throw new Exception("schema error", err);
                })
                //.UseExceptions()
                .AddAuthorization()
                .AddApolloTracing();

            services.AddSingleton(schemaBuilder);

            services.AddApplication<T>();
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

        public static IServiceCollection AddStorage(this IServiceCollection builder, string connectionStringName = "Geex")
        {
            var pack = new ConventionPack();
            pack.Add(new IgnoreExtraElementsConvention(true));
            ConventionRegistry.Register("My Solution Conventions", pack, t => true);
            builder.AddTransient(x => new MongoUrl(x.GetService<IConfiguration>().GetConnectionString(connectionStringName)));
            builder.AddTransient<IMongoClient>(x => new MongoClient(x.GetService<MongoUrl>()));
            builder.AddTransient(x => x.GetService<IMongoClient>().GetDatabase(x.GetService<MongoUrl>().DatabaseName));
            return builder;
        }
    }
}
