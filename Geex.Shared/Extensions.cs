using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Autofac;

using Geex.Shared.Roots.RootTypes;
using Geex.Shared.Types;

using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Voyager;
using HotChocolate.Execution;
using HotChocolate.Execution.Configuration;
using HotChocolate.Types;

using IdentityServer4.MongoDB.Configuration;
using IdentityServer4.MongoDB.Stores;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

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
                .AddQueryType<QueryType>()
                .AddMutationType<MutationType>()
                .AddSubscriptionType<SubscriptionType>()
                .AddType<ObjectIdType>()
                .AddErrorFilter(x =>
                {
                    Console.WriteLine(x);
                    return x;
                })
                .OnSchemaError((ctx, err) =>
                {
                    Console.WriteLine(ctx);
                    Console.WriteLine(err);
                })
                //.UseExceptions();
                .AddAuthorization()
                .AddApolloTracing();

            services.AddSingleton(schemaBuilder);
            //services.AddGraphQL(provider =>
            //{
            //    var schema = schemaBuilder
            //    .AddServices(provider)
            //    .AddAuthorizeDirectiveType()
            //    .Create();
            //    if (schema.QueryType.Fields.All(x => x.IsIntrospectionField) || schema.MutationType.Fields.All(x => x.IsIntrospectionField))
            //    {
            //        throw new Exception($"Query or Mutation must have one field at least!");
            //    }
            //    return schema;
            //});
            //services.AddQueryRequestInterceptor((context, builder, token) =>
            //{
            //    return Task.CompletedTask;
            //});

            services.AddApplication<T>();
        }
        public static void UseGeexGraphQL(this IApplicationBuilder app)
        {
            app.UseRouting()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapGraphQL();
                });
            app.UseVoyager("/graphql","/voyager");
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
                var extensionTypes = exportedTypes.Where(x => !x.IsAbstract && AbpTypeExtensions.IsAssignableTo<ObjectTypeExtension>(x)).ToArray();
                var objectTypes = exportedTypes.Where(x => !x.IsAbstract && AbpTypeExtensions.IsAssignableTo<IType>(x)).ToArray();
                foreach (var extensionType in extensionTypes)
                {
                    schemaBuilder.AddTypeExtension(extensionType);
                }
                return schemaBuilder
                    .AddTypes(objectTypes);
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

        public static IIdentityServerBuilder AddMongoRepository(this IIdentityServerBuilder builder,
            string connectionString)
        {
            builder.Services.AddSingleton<IOptions<MongoDBConfiguration>>(new OptionsWrapper<MongoDBConfiguration>(new MongoDBConfiguration()
            {
                ConnectionString = connectionString,
                Database = new MongoUrl(connectionString).DatabaseName
            }));
            builder.AddConfigurationStore(options =>
             {
                 options.ConnectionString = connectionString;
             })
            .AddOperationalStore(options =>
            {
                options.ConnectionString = connectionString;
            })
            .AddResourceStore<ResourceStore>()
            .AddClientStore<ClientStore>()
            .AddPersistedGrantStore<PersistedGrantStore>()
            ;

            return builder;
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
