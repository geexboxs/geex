using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.IO;
using Castle.Core;
using Geex.Shared._ShouldMigrateToLib;

namespace Geex.Core.UserManagement
{
    public class Role
    {
        public static IEqualityComparer<Role> Comparer = new GenericEqualityComparer<Role>().With(x => x.Name);
        public string Name { get; set; }

        public Role(string name)
        {
            this.Name = name;
        }
    }
}
