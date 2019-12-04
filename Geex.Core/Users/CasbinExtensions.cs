using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geex.Shared._ShouldMigrateToLib.Auth;

namespace Geex.Core.Users
{
    public static class CasbinExtensions
    {
        public static bool SetFeaturePolicy(this Enforcer _this, AppUser user, string[] features)
        {
            return _this.SetFeaturePolicy($"user.{user.Id}", features.Select(x => "feature." + x).ToArray());
        }

        public static bool SetFeaturePolicy(this Enforcer _this, Role role, string[] features)
        {
            return _this.SetFeaturePolicy($"user_group.{role.Id}", features.Select(x => "feature." + x).ToArray());
        }

        public static bool AddUserGroupPolicy(this Enforcer _this, AppUser user, string roleId)
        {
            return _this.AddUserGroupPolicy($"user.{user.Id}", $"user_group.{roleId}");
        }
        public static bool AddUserGroupPolicy(this Enforcer _this, AppUser user, Role role)
        {
            return _this.AddUserGroupPolicy($"user.{user.Id}", $"user_group.{role.Name}");
        }

        public static bool SetUserGroupPolicy(this Enforcer _this, AppUser user, IEnumerable<string> roleIds)
        {
            return _this.SetUserGroupPolicy($"user.{user.Id}", roleIds.Select(x => $"user_group.{x}"));
        }
        public static bool SetUserGroupPolicy(this Enforcer _this, AppUser user, IEnumerable<Role> roles)
        {
            return _this.SetUserGroupPolicy($"user.{user.Id}", roles.Select(x => $"user_group.{x.Id}"));
        }

        public static List<Policy> GetFeaturePolicies(this Enforcer _this, AppUser user)
        {
            return _this.GetFeaturePolicies($"user.{user.Id}");
        }

        public static List<Policy> GetFeaturePolicies(this Enforcer _this, Role role)
        {
            return _this.GetFeaturePolicies($"user_group.{role.Id}");
        }

        public static List<Policy> GetResourcePolicy(this Enforcer _this, AppUser user, string dataId)
        {
            return _this.GetResourcePolicy($"user.{user.Id}", $"data.{dataId}");

        }

        public static List<Policy> GetResourcePolicy(this Enforcer _this, Role role, string dataId)
        {
            return _this.GetResourcePolicy($"user_group.{role.Id}", $"data.{dataId}");

        }


        public static List<Enforcer.GroupPolicy> GetUserGroupPolicies(this Enforcer _this, AppUser user)
        {
            return _this.GetUserGroupPolicies($"user.{user.Id}");
        }

        public static bool Enforce(this Enforcer _this, AppUser user, string obj, string act = "*")
        {
            return _this.Enforce($"user.{user.Id}", $"feature.{obj}", act);
        }
        public static bool Enforce(this Enforcer _this, Role role, string obj, string act)
        {
            return _this.Enforce($"user_group.{role.Id}", obj, act);
        }
    }
}
