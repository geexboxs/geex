using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Geex.Shared;
using Geex.Shared._ShouldMigrateToLib;
using IdentityModel;
using MongoDB.Bson;

namespace Geex.Core.Users
{
    public class User
    {
        /// <summary>
        /// Gets or sets the subject identifier.
        /// </summary>
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the provider name.
        /// </summary>
        public string ProviderName { get; set; }

        /// <summary>
        /// Gets or sets the provider subject identifier.
        /// </summary>
        public string ProviderSubjectId { get; set; }

        /// <summary>
        /// Gets or sets if the user is active.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets the claims.
        /// </summary>
        public ICollection<Role> Roles { get; set; } = new HashSet<Role>(comparer: Role.Comparer);
        [Obsolete("This ctor should not be used in user code.")]
        public User()
        {

        }
        public User(string phoneOrEmail, string password, IQueryable<User> users, Func<User, string, string> passwordHasher, string username = null)
        {
            if (phoneOrEmail.IsValidEmail())
            {
                this.Email = phoneOrEmail;
            }
            else
            {
                this.PhoneNumber = phoneOrEmail;
            }
            this.Username = username ?? phoneOrEmail;
            this.CheckDuplicateUser(username, phoneOrEmail, phoneOrEmail, users);
            this.Password = passwordHasher.Invoke(this, password);
        }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        private void CheckDuplicateUser(string userName, string emailAddress, string phonenumber, IQueryable<User> users)
        {
            if (users
                .Any(o => o.Username == userName || o.Email == emailAddress || o.PhoneNumber == phonenumber))
            {
                throw new UserFriendlyException("UserAlreadyExists", "UserAlreadyExists_Msg", userName, emailAddress, phonenumber);
            }
            return;
        }
    }
}
