using HotChocolate.Types;

namespace Geex.Core.Authentication.GqlSchemas.Inputs
{
    public class AuthenticateInput
    {
        public string UserIdentifier { get; set; }
        public string Password { get; set; }
    }

    public class AssignRoleInputType : InputObjectType<AuthenticateInput>
    {

        protected override void Configure(IInputObjectTypeDescriptor<AuthenticateInput> descriptor)
        {
            descriptor.Field(t => t.UserIdentifier).Type<NonNullType<StringType>>();
            descriptor.Field(t => t.Password).Type<NonNullType<StringType>>();
        }
    }
}