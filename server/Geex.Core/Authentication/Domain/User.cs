using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CommonServiceLocator;

using Geex.Core.Authentication.Domain.Events;
using Geex.Core.Authorization;
using Geex.Core.Users;
using Geex.Shared;
using Geex.Shared._ShouldMigrateToLib;
using Geex.Shared._ShouldMigrateToLib.Abstractions;
using Geex.Shared._ShouldMigrateToLib.Auth;

using HotChocolate;
using HotChocolate.Types;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Entities;

namespace Geex.Core.Authentication.Domain
{
    public class User : GeexEntity
    {
        /// <summary>
        ///     Gets or sets the username.
        /// </summary>
        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public UserClaim[] Claims { get; set; }
        [InverseSide] public Many<Org> Orgs { get; set; }

        [OwnerSide]
        public Many<Role> Roles { get; protected set; }
        public List<AppPermission> AuthorizedPermissions { get; set; }
        protected User()
        {
            this.InitManyToMany(x => x.Roles, role => role.Users);
            this.InitManyToMany(x => x.Orgs, org => org.Users);
        }
        public User(IUserCreationValidator userCreationValidator, string phoneOrEmail, string password, string? username = null)
        : this()
        {
            if (phoneOrEmail.IsValidEmail())
                Email = phoneOrEmail;
            else if (phoneOrEmail.IsValidPhoneNumber())
                PhoneNumber = phoneOrEmail;
            else
                throw new Exception("invalid input for phoneOrEmail");
            var passwordHasher = ServiceLocator.Current.GetService<IPasswordHasher<User>>();
            UserName = username ?? phoneOrEmail;
            userCreationValidator.Check(this);
            Password = passwordHasher.HashPassword(this, password);
        }




        public bool CheckPassword(string password)
        {
            var passwordHasher = ServiceLocator.Current.GetService<IPasswordHasher<User>>();
            return passwordHasher!.VerifyHashedPassword(this, Password, password) == PasswordVerificationResult.Success;
        }

        public async Task AssignRoles(List<Role> roles)
        {
            await this.Roles.RemoveAsync(this.Roles.Select(x => x.Id));
            foreach (var role in roles)
            {
                await this.Roles.AddAsync(role);
            }
            await this.Roles.SaveAsync();
            this.AddDomainEvent(new UserRoleChangedEvent(this.Id, roles.Select(x => x.Id).ToList()));
        }
    }

    public interface IUserCreationValidator
    {
        public DbContext DbContext { get; }
        public void Check(User user)
        {
            if (DbContext.CountAsync<User>(o => o.UserName == user.UserName || o.Email == user.Email || o.PhoneNumber == user.PhoneNumber).Result > 0)
                throw new UserFriendlyException("UserAlreadyExists");
        }
    }
}