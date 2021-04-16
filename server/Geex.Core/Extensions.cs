using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Geex.Core.Authentication.Domain;

using MongoDB.Bson;
using MongoDB.Entities;

namespace Geex.Core
{
    public static class Extensions
    {
        public static Find<User, User> MatchUserIdentifier(this Find<User> @this, string userIdentifier)
        {
            if ((ObjectId.TryParse(userIdentifier, out _)))
            {
                return @this.Match(x => x.Email == userIdentifier || x.PhoneNumber == userIdentifier || x.Id == userIdentifier);
            }
            return @this.Match(x => x.Email == userIdentifier || x.PhoneNumber == userIdentifier);

        }
    }
}
