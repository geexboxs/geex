using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.IO;
using Castle.Core;
using Geex.Shared._ShouldMigrateToLib;
using MongoDB.Bson;
using System;
using Geex.Shared._ShouldMigrateToLib.Abstractions;
using Microsoft.AspNetCore.Http;
using Volo.Abp.Domain.Entities;

namespace Geex.Core.Users
{
    public class Role : Entity, IEquatable<Role>
    {
        public string Name { get; set; }

        public Role(string name)
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
            return HashCode.Combine(this.Name);
        }

        /// <summary>Implicitly calls ToString().</summary>
        /// <param name="path"></param>
        public static implicit operator string(Role path)
        {
            return path.Name;
        }

        /// <summary>
        /// Implicitly creates a new Role from the given string.
        /// </summary>
        /// <param name="s"></param>
        public static implicit operator Role(string s)
        {
            return new Role(s);
        }

        public static bool operator ==(Role left, Role right)
        {
            return EqualityComparer<Role>.Default.Equals(left, right);
        }

        public static bool operator !=(Role left, Role right)
        {
            return !(left == right);
        }

        public override object[] GetKeys()
        {
            return new[] { this.Name };
        }
    }
}
