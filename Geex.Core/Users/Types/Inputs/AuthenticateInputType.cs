using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geex.Core.Users.Inputs;
using HotChocolate.Types;

namespace Geex.Core.Users.Types.Inputs
{
    public class AuthenticateInputType : InputObjectType<AuthenticateInput>
    {

        protected override void Configure(IInputObjectTypeDescriptor<AuthenticateInput> descriptor)
        {
            descriptor.Field(t => t.UserIdentifier).Type<NonNullType<StringType>>().Description("username|phone|email");
            descriptor.Field(t => t.Password).Type<NonNullType<StringType>>();
            descriptor.Field(t => t.RedirectUri).Type<NonNullType<StringType>>();
        }
    }
}
