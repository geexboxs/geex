using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Geex.Shared.Roots.RootTypes;
using HotChocolate;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Geex.Shared
{
    public interface IGraphQLModule<out T> : IGraphQLModule
    {
    }
    public interface IGraphQLModule
    {
        void PreInitialize(IServiceCollection containerBuilder, SchemaBuilder schemaBuilder);
        void PostInitialize(IServiceProvider serviceProvider);
    }
    public abstract class GraphQLModule<T> : IGraphQLModule<T> where T : IGraphQLModule
    {

        public abstract void PostInitialize(IServiceProvider serviceProvider);

        /// <summary>
        /// This is the first event called on application startup.
        /// Codes can be placed here to run before dependency injection registrations.
        /// Please do not use `this` in the scope
        /// </summary>
        public virtual void PreInitialize(IServiceCollection containerBuilder, SchemaBuilder schemaBuilder)
        {
            schemaBuilder.AddModuleTypes<T>();
        }
    }

    public abstract class GraphQLEntryModule<T> : GraphQLModule<T> where T : IGraphQLModule
    {
        public List<Type> Resolvers { get; set; }

        public Type SubscriptionType { get; set; } = typeof(SubscriptionType);

        public Type MutationType { get; set; } = typeof(MutationType);

        public Type QueryType { get; set; } = typeof(QueryType);

    }

    public class GraphQLModuleOptions<T> : IGraphQLModuleOptions where T : IGraphQLModule
    {
    }

    public interface IGraphQLModuleOptions
    {
    }
}
