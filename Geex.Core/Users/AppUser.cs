using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Autofac;
using Geex.Shared;
using Geex.Shared._ShouldMigrateToLib;
using Geex.Shared._ShouldMigrateToLib.Auth;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using Repository.Mongo;

namespace Geex.Core.Users
{
    public class AppUser : Entity, IFunctionalEntity
    {
        public static Func<IComponentContext> ComponentContextResolver { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }
        public string PhoneNumber { get; set; }

        public string Email { get; set; }
        /// <summary>
        /// Gets or sets the claims.
        /// </summary>
        public ICollection<Role> Roles { get; set; } = new HashSet<Role>(comparer: Role.Comparer);
        public AppUser(AuthUser user)
        {
            this.Username = user.Username;
            this.Email = user.Email;
            this.PhoneNumber = user.PhoneNumber;
            this.Id = user.Id;
        }
    }
}
