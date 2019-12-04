using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Core.Misc;

namespace Geex.Shared._ShouldMigrateToLib
{
    public static class MongoCollectionExtensions
    {
        public static TDocument First<TDocument>(
            this IMongoCollection<TDocument> collection,
            Expression<Func<TDocument, bool>> filter,
            FindOptions options = null)
        {
            Ensure.IsNotNull(collection, nameof(collection));
            Ensure.IsNotNull(filter, nameof(filter));
            return collection.Find(new ExpressionFilterDefinition<TDocument>(filter), options).First();
        }

        public static TDocument FirstOrDefault<TDocument>(
            this IMongoCollection<TDocument> collection,
            Expression<Func<TDocument, bool>> filter,
            FindOptions options = null)
        {
            Ensure.IsNotNull(collection, nameof(collection));
            Ensure.IsNotNull(filter, nameof(filter));
            return collection.Find(new ExpressionFilterDefinition<TDocument>(filter), options).FirstOrDefault();
        }

        public static Task<TDocument> FirstAsync<TDocument>(
            this IMongoCollection<TDocument> collection,
            Expression<Func<TDocument, bool>> filter,
            FindOptions options = null)
        {
            Ensure.IsNotNull(collection, nameof(collection));
            Ensure.IsNotNull(filter, nameof(filter));
            return collection.Find(new ExpressionFilterDefinition<TDocument>(filter), options).FirstAsync();
        }

        public static Task<TDocument> FirstOrDefaultAsync<TDocument>(
            this IMongoCollection<TDocument> collection,
            Expression<Func<TDocument, bool>> filter,
            FindOptions options = null)
        {
            Ensure.IsNotNull(collection, nameof(collection));
            Ensure.IsNotNull(filter, nameof(filter));
            return collection.Find(new ExpressionFilterDefinition<TDocument>(filter), options).FirstOrDefaultAsync();
        }
    }
}
