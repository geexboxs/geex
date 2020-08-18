using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.IO;
using Castle.Core;
using Geex.Shared._ShouldMigrateToLib;
using MongoDB.Bson;
using System;

namespace Geex.Core.Users
{
    public class Role : NetCasbin.Rbac.Role, IEquatable<Role>
    {
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the subject identifier.
        /// </summary>
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

        public Role(string name) : base(name)
        {
            this.Name = name;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Role);
        }

        public bool Equals(Role other)
        {
            return other != null &&
                   this.Name == other.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Name, this.Id);
        }

        public static bool operator ==(Role left, Role right)
        {
            return EqualityComparer<Role>.Default.Equals(left, right);
        }

        public static bool operator !=(Role left, Role right)
        {
            return !(left == right);
        }
    }
}
