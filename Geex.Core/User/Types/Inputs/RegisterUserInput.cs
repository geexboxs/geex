using HotChocolate.Types;

namespace Geex.Core.User.Types.Inputs
{
    public class RegisterUserInputType : InputObjectType<RegisterUserInputType.RegisterUserInput>
    {
        public class RegisterUserInput
        {
            public string UserName { get; set; }
        }
        protected override void Configure(IInputObjectTypeDescriptor<RegisterUserInput> descriptor)
        {
            descriptor.Field(t => t.UserName).Type<NonNullType<StringType>>();
        }
    }
}