using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using MongoDB.Bson;
using MongoDB.Driver;
using Repository.Mongo;

namespace Geex.Shared._ShouldMigrateToLib.Auth
{
    public class GeexPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly Repository<AuthUser> _userCollection;
        private readonly Enforcer _enforcer;

        public GeexPasswordValidator(Repository<AuthUser> userCollection, Enforcer enforcer)
        {
            _userCollection = userCollection;
            _enforcer = enforcer;
        }
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = _userCollection.First(x => (x.Username == context.UserName || x.Email == context.UserName || x.PhoneNumber == context.UserName));
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