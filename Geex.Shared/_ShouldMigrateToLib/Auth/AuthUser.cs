using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using CommonServiceLocator;
using Geex.Shared._ShouldMigrateToLib.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Geex.Shared._ShouldMigrateToLib.Auth
{
    public class AuthUser : ActiveRecordAggregateRoot<AuthUser>
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }
        public string PhoneNumber { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }

        public AuthUser(string phoneOrEmail, string password, string username = null)
        {
            var passwordHasher = ServiceLocator.Current.GetService<IPasswordHasher<AuthUser>>();
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
            var users = this.Repository;
            if (users
                .Any(o => o.Username == this.Username || o.Email == this.Email || o.PhoneNumber == this.PhoneNumber))
            {
                throw new UserFriendlyException("UserAlreadyExists", "UserAlreadyExists_Msg", this.Username, this.Email, this.PhoneNumber);
            }
            return;
        }
        public bool CheckPassword(string password)
        {
            var passwordHasher = ServiceLocator.Current.GetService<IPasswordHasher<AuthUser>>();
            return passwordHasher.VerifyHashedPassword(this, this.Password, password) == PasswordVerificationResult.Success;
        }
    }
}