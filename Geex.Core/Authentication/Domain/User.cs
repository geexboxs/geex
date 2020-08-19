using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
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
using MongoDB.Driver;
using Volo.Abp.Domain.Repositories;

namespace Geex.Shared._ShouldMigrateToLib.Auth
{
    public class User : ActiveRecordAggregateRoot<User>
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }
        public string PhoneNumber { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public ImmutableList<string> Roles { get; set; }
        public IQueryable<UserClaimRef> Claims => ServiceLocator.Current.GetService<IRepository<UserClaimRef>>().Where(x => x.UserId == this.Id);
        public IQueryable<UserOrgRef> Orgs => ServiceLocator.Current.GetService<IRepository<UserOrgRef>>().Where(x => x.UserId == this.Id);

        public User(string phoneOrEmail, string password, string username = null)
        {
            var passwordHasher = ServiceLocator.Current.GetService<IPasswordHasher<User>>();
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
            var users = this.As<IActiveRecord<User>>().Repository;
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

        public User SetOrgs(List<string> orgs)
        {
            
            IActiveRecord<UserOrgRef>.StaticRepository.DeleteAsync(x => x.UserId == this.Id);
            return this;
        }
    }
}