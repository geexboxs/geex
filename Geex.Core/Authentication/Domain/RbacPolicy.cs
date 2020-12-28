using System.Collections.Generic;

namespace Geex.Shared._ShouldMigrateToLib.Auth
{
    public class RbacPolicy
    {
        public RbacPolicy(List<string> x)
        {
            this.Sub = x[0];
            this.Obj = x[1];
            this.Act = x[2];
        }

        public RbacPolicy(string sub, string obj, string act)
        {
            Sub = sub;
            Obj = obj;
            Act = act;
        }
        public string Sub { get; }
        public string Obj { get; }
        public string Act { get; }
    }
}