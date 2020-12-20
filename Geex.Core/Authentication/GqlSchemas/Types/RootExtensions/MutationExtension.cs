using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geex.Core.Authentication.GqlSchemas.Inputs;
using Geex.Shared.Roots;
using HotChocolate.Types;

namespace Geex.Core.Authentication.GqlSchemas.Types.RootExtensions
{
    public class MutationExtension:ObjectTypeExtension<Mutation>
    {
        protected override void Configure(IObjectTypeDescriptor<Mutation> descriptor)
        {
            descriptor.Field<AuthenticationResolver>(x => x.Authenticate(default, default, default))
                .Argument("input", x => x.Type<AssignRoleInputType>());
            base.Configure(descriptor);
        }
    }
}
