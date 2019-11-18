using HotChocolate.Types;

namespace Geex.Core.UserManagement.Types.Inputs
{
    public class RegisterUserInputType : InputObjectType<RegisterUserInput>
    {

        protected override void Configure(IInputObjectTypeDescriptor<RegisterUserInput> descriptor)
        {
            descriptor.Field(t => t.UserName).Type<NonNullType<StringType>>();
            descriptor.Field(t => t.PhoneOrEmail).Type<NonNullType<StringType>>();
            descriptor.Field(t => t.Password).Type<NonNullType<StringType>>();
        }
    }
}