using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Geex.Shared.Roots.RootTypes;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Voyager;
using HotChocolate.Execution;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Geex.Shared
{
    public static class Extensions
    {
        public static void AddGeexGraphQL<T>(this ContainerBuilder containerBuilder, Action<IQueryExecutionBuilder> build = null) where T : GraphQLEntryModule<T>
        {
            var schemaBuilder = SchemaBuilder.New();
            var schemaFactory = new Func<IServiceProvider, ISchema>(provider =>
            {
                var schema = schemaBuilder
                .AddServices(provider)
                .AddAuthorizeDirectiveType()
                .AddQueryType<QueryType>()
                .AddMutationType<MutationType>()
                .AddSubscriptionType<SubscriptionType>()
                .Create();
                if (schema.QueryType.Fields.All(x => x.IsIntrospectionField) || schema.MutationType.Fields.All(x => x.IsIntrospectionField))
                {
                    throw new Exception($"Query or Mutation must have one field at least!");
                }
                return schema;
            });
            IServiceCollection serviceCollection = new ServiceCollection();
            if (build == null)
            {
                serviceCollection.AddGraphQL(schemaFactory);
            }
            else
            {
                serviceCollection.AddGraphQL(schemaFactory, build);
            }
            serviceCollection.AddSingleton(schemaBuilder);
            containerBuilder.Populate(serviceCollection);
            RegisterModule(typeof(T), containerBuilder, schemaBuilder);

        }
        public static void UseGeexGraphQL(this IApplicationBuilder app)
        {
            app.UseGraphQL();
            app.UseVoyager();
            app.UsePlayground();
        }

        private static void RegisterModule(Type module, ContainerBuilder containerBuilder,
            SchemaBuilder schemaBuilder)
        {
            var dependModules = module.GetCustomAttribute<DependsOnAttribute>()?.DependedModuleTypes;
            if (dependModules != null)
            {
                foreach (var dependModule in dependModules)
                {
                    RegisterModule(dependModule, containerBuilder, schemaBuilder);
                }
            }

            var ctor = module.GetMatchingConstructor(new Type[] { });
            if (ctor == null)
            {
                throw new Exception($"Cannot construct module:{module}, please ensure a public default constructor for module class.");
            }

            var moduleInstance = ctor.Invoke(null) as IGraphQLModule;
            moduleInstance.PreInitialize(containerBuilder, schemaBuilder);
            containerBuilder.Register(x =>
            {
                moduleInstance.PostInitialize(x);
                return moduleInstance;
            });
        }

        public static ISchemaBuilder AddModuleTypes<TModule>(this SchemaBuilder schemaBuilder)
        {
            return schemaBuilder
                .AddTypes(typeof(TModule).Assembly.GetExportedTypes().Where(x => x.Namespace != null && x.Namespace.Contains($"{typeof(TModule).Namespace}.Types")).ToArray());
        }

        public static bool IsValidEmail(this string str)
        {
            return new Regex(@"\w[-\w.+]*@([A-Za-z0-9][-A-Za-z0-9]+\.)+[A-Za-z]{2,14}").IsMatch(str);
        }

        public static IIdentityServerBuilder AddMongoRepository(this IIdentityServerBuilder builder)
        {
            builder.Services.AddTransient<IMongoClient, MongoClient>();

            return builder;
        }
    }
}
