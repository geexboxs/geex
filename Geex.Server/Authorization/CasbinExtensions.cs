using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using NetCasbin.Model;

namespace Geex.Core.Users
{
    public static class CasbinExtensions
    {
        public static void AddCasbinAuthorization(this IServiceCollection services)
        {
            // Replace the default authorization policy provider with our own
            // custom provider which can return authorization policies for given
            // policy names (instead of using the default policy provider)
            services.AddSingleton<IAuthorizationPolicyProvider, CasbinAuthorizationPolicyProvider>();

            // As always, handlers must be provided for the requirements of the authorization policies
            services.AddScoped<IAuthorizationHandler, CasbinAuthorizationHandler>();
        }
        public static Model LoadFromText(this Model model, string text)
        {
            model.LoadModelFromText(text);
            return model;
        }

        public static bool SetFeaturePolicy(this Enforcer _this, User user, string[] features)
        {
            return _this.SetFeaturePolicy($"user.{user.Id}", features.Select(x => "feature." + x).ToArray());
        }

        public static bool SetFeaturePolicy(this Enforcer _this, Role role, string[] features)
        {
            return _this.SetFeaturePolicy($"user_group.{role.Id}", features.Select(x => "feature." + x).ToArray());
        }

        public static bool AddUserGroupPolicy(this Enforcer _this, User user, string roleId)
        {
            return _this.AddUserGroupPolicy($"user.{user.Id}", $"user_group.{roleId}");
        }
        public static bool AddUserGroupPolicy(this Enforcer _this, User user, Role role)
        {
            return _this.AddUserGroupPolicy($"user.{user.Id}", $"user_group.{role.Name}");
        }

        public static bool SetUserGroupPolicy(this Enforcer _this, User user, IEnumerable<string> roleIds)
        {
            return _this.SetUserGroupPolicy($"user.{user.Id}", roleIds.Select(x => $"user_group.{x}"));
        }
        public static bool SetUserGroupPolicy(this Enforcer _this, User user, IEnumerable<Role> roles)
        {
            return _this.SetUserGroupPolicy($"user.{user.Id}", roles.Select(x => $"user_group.{x.Id}"));
        }

        public static List<Policy> GetFeaturePolicies(this Enforcer _this, User user)
        {
            return _this.GetFeaturePolicies($"user.{user.Id}");
        }

        public static List<Policy> GetFeaturePolicies(this Enforcer _this, Role role)
        {
            return _this.GetFeaturePolicies($"user_group.{role.Id}");
        }

        public static List<Policy> GetResourcePolicy(this Enforcer _this, User user, string dataId)
        {
            return _this.GetResourcePolicy($"user.{user.Id}", $"data.{dataId}");

        }

        public static List<Policy> GetResourcePolicy(this Enforcer _this, Role role, string dataId)
        {
            return _this.GetResourcePolicy($"user_group.{role.Id}", $"data.{dataId}");

        }


        public static List<Enforcer.GroupPolicy> GetUserGroupPolicies(this Enforcer _this, User user, Role role)
        {
            return _this.GetUserGroupPolicies($"user.{user.Id}", $"user_group.{role.Id}");
        }

        public static bool Enforce(this Enforcer _this, User user, string obj, string act = "*")
        {
            return _this.Enforce($"user.{user.Id}", $"feature.{obj}", act);
        }
        public static bool Enforce(this Enforcer _this, Role role, string obj, string act)
        {
            return _this.Enforce($"user_group.{role.Id}", obj, act);
        }
    }
}