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
    public abstract class GeexEntity : IEntity, ICreatedOn, IModifiedOn
    {
        [BsonId]
        [ObjectId]
        public string ID { get; set; }

        string IEntity.GenerateNewID()
        {
            return ObjectId.GenerateNewId().ToString();
        }

        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
