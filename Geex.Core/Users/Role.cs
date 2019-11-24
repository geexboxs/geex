using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.IO;
using Castle.Core;
using Geex.Shared._ShouldMigrateToLib;
using MongoDB.Bson;

namespace Geex.Core.Users
{
    public class Role
    {
        public static IEqualityComparer<Role> Comparer = new GenericEqualityComparer<Role>().With(x => x.Name);
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the subject identifier.
        /// </summary>
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

        public Role(string name)
        {
            this.Name = name;
        }
    }
}
