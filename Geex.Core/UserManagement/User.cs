using System;
using System.Collections.Generic;
using System.Security.Claims;
using Geex.Shared;
using IdentityModel;

namespace Geex.Core.UserManagement
{
    public class User
    {
        /// <summary>
        /// Gets or sets the subject identifier.
        /// </summary>
        public string Id { get; set; }

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
        public ICollection<Claim> Claims { get; set; } = new HashSet<Claim>(new ClaimComparer());
        [Obsolete("This ctor should not be used in user code.")]
        public User()
        {

        }
        public User(string phoneOrEmail, string password, string username = null, Func<string, string> passwordHasher = null)
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
        }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }
    }
}
