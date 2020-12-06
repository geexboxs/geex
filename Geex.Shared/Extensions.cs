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

            var schemaBuilder = SchemaBuilder.New()
                .AddQueryType<QueryType>()
                .AddMutationType<MutationType>()
                .AddSubscriptionType<SubscriptionType>()
                .AddType<ObjectIdType>();
            services.AddGraphQL(provider =>
            {
                var schema = schemaBuilder
                .AddServices(provider)
                .AddAuthorizeDirectiveType()
                .Create();
                if (schema.QueryType.Fields.All(x => x.IsIntrospectionField) || schema.MutationType.Fields.All(x => x.IsIntrospectionField))
                {
                    throw new Exception($"Query or Mutation must have one field at least!");
                }
                return schema;
            });
            //services.AddQueryRequestInterceptor((context, builder, token) =>
            //{
            //    return Task.CompletedTask;
            //});

            services.AddSingleton(schemaBuilder);
            services.AddApplication<T>();
        }
        public static void UseGeexGraphQL(this IApplicationBuilder app)
        {
            app.UseGraphQL();
            app.UseVoyager();
            app.UsePlayground();
        }

        public static ISchemaBuilder AddModuleTypes<TModule>(this ISchemaBuilder schemaBuilder)
        {
            return schemaBuilder
                .AddTypes(typeof(TModule).Assembly.GetExportedTypes().Where(x => x.Namespace != null && x.Namespace.Contains($"{typeof(TModule).Namespace}.GqlSchemas")).ToArray());
        }

        public static ISchemaBuilder AddModuleTypes(this ISchemaBuilder schemaBuilder,Type gqlModuleType)
        {
            return schemaBuilder
                .AddTypes(gqlModuleType.Assembly.GetExportedTypes().Where(x => !x.IsAbstract && AbpTypeExtensions.IsAssignableTo<IType>(x)).ToArray());
        }

        public static bool IsValidEmail(this string str)
        {
            return new Regex(@"\w[-\w.+]*@([A-Za-z0-9][-A-Za-z0-9]+\.)+[A-Za-z]{2,14}").IsMatch(str);
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
