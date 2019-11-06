using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Geex.Shared.Roots.RootTypes;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;
using BindingFlags = System.Reflection.BindingFlags;

namespace Geex.Shared
{
    public static class Extensions
    {
        public static void AddGeexGraphQL<T>(this IServiceCollection containerBuilder, Action<IQueryExecutionBuilder> build = null) where T : GraphQLEntryModule<T>
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
            if (build == null)
            {
                containerBuilder.AddGraphQL(schemaFactory);
            }
            else
            {
                containerBuilder.AddGraphQL(schemaFactory, build);
            }
            containerBuilder.AddSingleton(schemaBuilder);

            RegisterModule(typeof(T), containerBuilder, schemaBuilder);

        }

        private static void RegisterModule(Type module, IServiceCollection containerBuilder,
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
            containerBuilder.AddSingleton(provider =>
                {
                    moduleInstance.PostInitialize();
                    return moduleInstance;
                });
        }

        public static ISchemaBuilder AddModuleTypes<TModule>(this SchemaBuilder schemaBuilder)
        {
            return schemaBuilder
                .AddTypes(typeof(TModule).Assembly.GetExportedTypes().Where(x => x.Namespace != null && x.Namespace.Contains($"{typeof(TModule).Namespace}.Types")).ToArray());
        }

    }
}
