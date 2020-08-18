using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

using Autofac;

using CommonServiceLocator;

using Geex.Core.Users;
using Geex.Shared._ShouldMigrateToLib.Abstractions;

using IdentityServer4.MongoDB.Entities;
using IdentityServer4.Stores.Serialization;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using MongoDB.Bson;
using MongoDB.Driver;

using Volo.Abp.Domain.Entities;

namespace Geex.Shared._ShouldMigrateToLib.Auth
{
    public class AppUser : ActiveRecordAggregateRoot<AppUser>
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }
        public string PhoneNumber { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public IQueryable<Role> Roles { get; set; }
        public IQueryable<ClaimLite> Claims { get; set; }

        public AppUser(string phoneOrEmail, string password, string username = null)
        {
            var passwordHasher = ServiceLocator.Current.GetService<IPasswordHasher<AppUser>>();
            if (phoneOrEmail.IsValidEmail())
            {
                this.Email = phoneOrEmail;
            }
            else
            {
                this.PhoneNumber = phoneOrEmail;
            }
            this.Username = username ?? phoneOrEmail;
            this.Password = passwordHasher.HashPassword(this, password);
            this.CheckDuplicateUser();
        }


        private void CheckDuplicateUser()
        {
            var users = this.As<IActiveRecord<AppUser>>().Repository;
            if (users
                .Any(o => o.Username == this.Username || o.Email == this.Email || o.PhoneNumber == this.PhoneNumber))
            {
                throw new UserFriendlyException("UserAlreadyExists", "UserAlreadyExists_Msg", this.Username, this.Email, this.PhoneNumber);
            }
            return;
        }
        public bool CheckPassword(string password)
        {
            var passwordHasher = ServiceLocator.Current.GetService<IPasswordHasher<AppUser>>();
            return passwordHasher.VerifyHashedPassword(this, this.Password, password) == PasswordVerificationResult.Success;
        }
    }
}