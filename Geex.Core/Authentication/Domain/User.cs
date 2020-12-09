using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Claims;

using Autofac;

using CommonServiceLocator;

using Geex.Core.Authorization;
using Geex.Core.Users;
using Geex.Shared._ShouldMigrateToLib.Abstractions;

using IdentityServer4.MongoDB.Entities;
using IdentityServer4.Stores.Serialization;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using MongoDB.Driver;
using MongoDB.Entities;

namespace Geex.Shared._ShouldMigrateToLib.Auth
{
    public class User : Entity, ICreatedOn, IModifiedOn
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }
        public string PhoneNumber { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public UserClaim[] Claims { get; set; }
        [InverseSide]
        public Many<Org> Orgs { get; set; }

        [InverseSide]
        public Many<Role> Roles { get; set; }
        public List<AppPermission> AuthorizedPermissions { get; set; }
        public User(string phoneOrEmail, string password, string username = null)
        {
            this.InitManyToMany(() => Orgs, org => org.Users);
            this.InitManyToMany(() => Roles, role => role.Users);
            var passwordHasher = ServiceLocator.Current.GetService<IPasswordHasher<User>>();
            if (phoneOrEmail.IsValidEmail())
            {
                this.Email = phoneOrEmail;
            }
            else if (phoneOrEmail.IsValidPhoneNumber())
            {
                this.PhoneNumber = phoneOrEmail;
            }
            else
            {
                throw new Exception("invalid input for phoneOrEmail");
            }
            this.Username = username ?? phoneOrEmail;
            this.Password = passwordHasher.HashPassword(this, password);
            this.CheckDuplicateUser();
        }


        private void CheckDuplicateUser()
        {
            var users = DB.Collection<User>().AsQueryable();
            if (users
                .Any(o => o.Username == this.Username || o.Email == this.Email || o.PhoneNumber == this.PhoneNumber))
            {
                throw new UserFriendlyException("UserAlreadyExists", "UserAlreadyExists_Msg", this.Username, this.Email, this.PhoneNumber);
            }
            return;
        }
        public bool CheckPassword(string password)
        {
            var passwordHasher = ServiceLocator.Current.GetService<IPasswordHasher<User>>();
            return passwordHasher.VerifyHashedPassword(this, this.Password, password) == PasswordVerificationResult.Success;
        }

        public string ID { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}