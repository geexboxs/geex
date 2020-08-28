using System.Collections.Generic;
using System.Linq;
using NetCasbin.Model;
using Policy = Geex.Shared._ShouldMigrateToLib.Auth.Policy;

namespace Geex.Core.Authorization.Casbin
{
    public class Enforcer
    {
        private NetCasbin.Enforcer internalEnforcer;
        public Enforcer(CasbinMongoAdapter adapter)
        {
            internalEnforcer = new NetCasbin.Enforcer(Model, adapter);
        }
        /// <summary>
        /// # defines
        /// p, user.1, data.1, read
        /// p, user.2, data.2, write
        /// p, user_group.1, data_group.1, write
        /// 
        /// g, user.1, user_group.1
        /// g2, data.1, data_group.1
        /// g2, data.2, data_group.1
        ///
        /// # requests
        /// user.1, data.1, read : true
        /// user.1, data.1, write : true
        /// user.1, data.2, read : false
        /// user.1, data.2, write : true
        /// </summary>
        public static Model Model { get; } = new Model().LoadFromText(@"
[request_definition]
r = sub, obj, act

[policy_definition]
p = sub, obj, act

[role_definition]
g = _, _
g2 = _, _

[policy_effect]
e = some(where (p.eft == allow))

[matchers]
m = (p.sub == ""*"" || g(r.sub, p.sub)) && (p.obj == ""*"" || g2(r.obj, p.obj)) && (p.act == ""*"" || r.act == p.act)
");



        public bool AddUserGroupPolicy(string sub, string sub_group)
        {
            return internalEnforcer.AddNamedGroupingPolicy("g", sub, sub_group);
        }



        public bool SetUserGroupPolicy(string sub, IEnumerable<string> sub_groups)
        {
            var result = true;
            internalEnforcer.RemoveNamedGroupingPolicy("g", sub);
            foreach (var sub_group in sub_groups)
            {
                result = result && internalEnforcer.AddNamedGroupingPolicy("g", sub, sub_group);
            }
            return result;
        }


        public List<GroupPolicy> GetUserGroupPolicies(string sub)
        {
            return internalEnforcer.GetFilteredNamedGroupingPolicy("g", 0, sub).Select(x => new GroupPolicy(x)).ToList();
        }

        public bool AddResourceGroupPolicy(string resourceId, string groupId)
        {
            return internalEnforcer.AddNamedGroupingPolicy("g2", $"{resourceId}", $"{groupId}");
        }

        public bool SetFeaturePolicy(string sub, string[] objs)
        {
            var result = true;
            internalEnforcer.RemoveFilteredNamedPolicy("p", 0, sub);
            foreach (var obj in objs)
            {
                result = result && internalEnforcer.AddNamedPolicy("p", sub, obj, "*");
            }
            return result;
        }

        public bool SetResourcePolicy(string sub, string obj, string[] acts)
        {
            var result = true;
            internalEnforcer.RemoveNamedPolicy("p", sub, obj);
            foreach (var act in acts)
            {
                result = result && internalEnforcer.AddNamedPolicy("p", sub, obj, act);
            }

            return result;
        }

        public List<Policy> GetFeaturePolicies(string sub)
        {
            var policies = internalEnforcer.GetFilteredNamedPolicy("p", 1, sub);
            return policies.Select(x => new Policy(x)).ToList();
        }

        public List<Policy> GetResourcePolicy(string sub, string obj)
        {
            var policies = internalEnforcer.GetFilteredNamedPolicy("p", 2, sub, obj);
            return policies.Select(x => new Policy(x)).ToList();
        }

        public bool HasRoleForUser(string user, string role)
        {
            return internalEnforcer.HasRoleForUser(user, role);
        }

        public bool Enforce(string sub, string obj, string act = "*")
        {
            return internalEnforcer.Enforce(sub, obj, act);
        }

        public bool DeleteResourceGroupPolicy(string resourceOrGroupName)
        {
            return internalEnforcer.RemoveFilteredNamedGroupingPolicy("g2", 0, resourceOrGroupName);
        }

        public class GroupPolicy
        {
            public GroupPolicy(List<string> x)
            {
                this.Sub = x[0];
                this.Group = x[1];
            }

            public GroupPolicy(string sub, string group)
            {
                Sub = sub;
                Group = @group;
            }

            public string Sub { get; }
            public string Group { get; }
        }


    }
}