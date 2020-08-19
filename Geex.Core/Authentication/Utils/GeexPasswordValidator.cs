using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using MongoDB.Bson;
using MongoDB.Driver;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.MongoDB;

namespace Geex.Shared._ShouldMigrateToLib.Auth
{
    public class GeexPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IMongoDbRepository<User> _userRepository;
        private readonly Enforcer _enforcer;

        public GeexPasswordValidator(IMongoDbRepository<User> userRepository, Enforcer enforcer)
        {
            _userRepository = userRepository;
            _enforcer = enforcer;
        }
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = _userRepository.First(x => (x.Username == context.UserName || x.Email == context.UserName || x.PhoneNumber == context.UserName));
            if (user != default)
            {
                if (user.CheckPassword(context.Password))
                {
                    context.Result = new GrantValidationResult(user.Id.ToString(), GrantType.ResourceOwnerPassword, DateTime.Now, new[]
                    {
                        new Claim("permissions",_enforcer.GetFeaturePolicies(user.Id.ToString()).Select(x=>x.Obj).ToJson()),
                        new Claim("role",_enforcer.GetUserGroupPolicies(user.Id.ToString()).Select(x=>x.Group).ToJson()),
                    });
                    return;
                }
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "password incorrect");
            }
            else
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "user not exist");
            }
        }
    }
}