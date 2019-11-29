using System.Collections.Generic;

namespace Geex.Shared._ShouldMigrateToLib.Authorization
{
    public class Policy
    {
        public Policy(List<string> x)
        {
            this.Sub = x[0];
            this.Obj = x[1];
            this.Act = x[2];
        }

        public Policy(string sub, string obj, string act)
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