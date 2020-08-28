using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Geex.Core.Users;
using HotChocolate;
using HotChocolate.Language;
using HotChocolate.Types;

namespace Geex.Core.UserManagement.GqlSchemas.Types
{
    public class RoleType : ObjectType<Role>
    {
        protected override void Configure(IObjectTypeDescriptor<Role> descriptor)
        {
            base.Configure(descriptor);
            descriptor.Field(x => x.Name);
        }
    }

    public abstract class RegexStringType : ScalarType
    {
        protected RegexStringType(NameString name, string pattern) : base(name)
        {
            this.Regex = new Regex(pattern, RegexOptions.Compiled);
        }

        public Regex Regex { get; set; }
    }
}
