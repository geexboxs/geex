using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

namespace Geex.Shared._ShouldMigrateToLib.Abstractions
{
    public interface IGeexEntity : IEntity, ICreatedOn
    {
        /// <summary>
        /// The ID property for this entity type.
        /// <para>IMPORTANT: make sure to decorate this property with the [BsonId] attribute when implementing this interface</para>
        /// </summary>
        string IEntity.ID
        {
            get => this.Id;
            set => this.Id = value;
        }

        [BsonId]
        [ObjectId]
        string Id { get; set; }

        /// <summary>
        /// Generate and return a new ID string from this method. It will be used when saving new entities that don't have their ID set.
        /// That is, if an entity has a null ID, this method will be called for getting a new ID value.
        /// If you're not doing custom ID generation, simply do <c>return ObjectId.GenerateNewId().ToString()</c>
        /// </summary>
        string GenerateNewID()
        {
            return ObjectId.GenerateNewId().ToString();
        }
    }
}
