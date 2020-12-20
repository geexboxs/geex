using System;
using System.Collections.Generic;
using System.Linq;

using CommonServiceLocator;

using Geex.Core.Authorization;
using Geex.Core.Users;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using MongoDB.Driver;
using MongoDB.Entities;

namespace Geex.Shared._ShouldMigrateToLib.Auth
{
    public class User : Entity, ICreatedOn, IModifiedOn
    {
        /// <summary>
        ///     Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public UserClaim[] Claims { get; set; }
        [InverseSide] public Many<Org> Orgs { get; set; }

        [OwnerSide] public Many<Role> Roles { get; set; }
        public List<AppPermission> AuthorizedPermissions { get; set; }
        public User()
        {
            this.InitManyToMany(() => Roles, role => role.Users);
            this.InitManyToMany(() => Orgs, org => org.Users);
        }
        public User(string phoneOrEmail, string password, string username = null)
        : this()
        {
            var passwordHasher = ServiceLocator.Current.GetService<IPasswordHasher<User>>();
            if (phoneOrEmail.IsValidEmail())
                Email = phoneOrEmail;
            else if (phoneOrEmail.IsValidPhoneNumber())
                PhoneNumber = phoneOrEmail;
            else
                throw new Exception("invalid input for phoneOrEmail");
            Username = username ?? phoneOrEmail;
            Password = passwordHasher.HashPassword(this, password);
            CheckDuplicateUser();
        }


        private void CheckDuplicateUser()
        {
            var users = DB.Collection<User>().AsQueryable();
            if (users
                .Any(o => o.Username == Username || o.Email == Email || o.PhoneNumber == PhoneNumber))
                throw new UserFriendlyException("UserAlreadyExists", "UserAlreadyExists_Msg", Username, Email,
                    PhoneNumber);
        }

        public bool CheckPassword(string password)
        {
            var passwordHasher = ServiceLocator.Current.GetService<IPasswordHasher<User>>();
            return passwordHasher.VerifyHashedPassword(this, Password, password) == PasswordVerificationResult.Success;
        }

        public string ID { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}