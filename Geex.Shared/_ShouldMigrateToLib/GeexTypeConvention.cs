using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HotChocolate.Types.Descriptors;

namespace Geex.Shared._ShouldMigrateToLib
{
    public class GeexTypeConvention: DefaultTypeInspector
    {
        protected override void Initialize(IConventionContext context)
        {
            base.Initialize(context);
        }

        public override IEnumerable<MemberInfo> GetMembers(Type type)
        {
            return base.GetMembers(type);
        }

        protected override void Complete(IConventionContext context)
        {
            base.Complete(context);
        }
    }
}
