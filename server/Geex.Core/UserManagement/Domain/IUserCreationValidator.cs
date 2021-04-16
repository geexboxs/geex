using System.Linq;

using Geex.Core.Authentication.Domain;
using Geex.Shared._ShouldMigrateToLib;

using MongoDB.Entities;

using GeexExceptionType = Geex.Shared.GeexExceptionType;

namespace Geex.Core.UserManagement.Domain
{
    public interface IUserCreationValidator
    {
        public DbContext DbContext { get; }
        public void Check(User user)
        {
            if (DbContext.Find<User>().Match(o => o.Email == user.Email || o.PhoneNumber == user.PhoneNumber).Project(x => x.Include(y => y.Id)).ExecuteAsync().Result.Any())
                throw new GeexUserFriendlyException(GeexExceptionType.UserAlreadyExists);
        }
    }
}