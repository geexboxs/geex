using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Geex.Shared.Roots.RootTypes;
using Geex.Shared.Types;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Voyager;
using HotChocolate.Execution;
using HotChocolate.Types;
using IdentityServer4.MongoDB.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Geex.Shared
{
    public static class Extensions
    {
        public static void AddGeexGraphQL<T>(this ContainerBuilder containerBuilder) where T : GraphQLEntryModule<T>
        {
            var schemaBuilder = SchemaBuilder.New()
                .AddQueryType<QueryType>()
                .AddMutationType<MutationType>()
                .AddSubscriptionType<SubscriptionType>()
                .AddType<ObjectIdType>();
            var services = new ServiceCollection();
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
            services.AddQueryRequestInterceptor((context, builder, token) =>
            {
                return Task.CompletedTask;
            });

            services.AddSingleton(schemaBuilder);
            containerBuilder.Populate(services);
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
                .AddTypes(typeof(TModule).Assembly.GetExportedTypes().Where(x => x.Namespace != null && x.Namespace.Contains($"{typeof(TModule).Namespace}.Types")).ToArray());
        }

        public static ISchemaBuilder AddModuleTypes(this ISchemaBuilder schemaBuilder,Type gqlModuleType)
        {
            return schemaBuilder
                .AddTypes(gqlModuleType.Assembly.GetExportedTypes().Where(x => x.Namespace != null && x.Namespace.Contains($"{gqlModuleType.Namespace}.Types")).ToArray());
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
            });

            return builder;
        }
    }
}
