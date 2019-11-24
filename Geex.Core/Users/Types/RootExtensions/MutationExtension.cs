using Geex.Core.Users.Types.Inputs;
using Geex.Shared.Roots;
using HotChocolate.Types;

namespace Geex.Core.Users.Types.RootExtensions
{
    public class MutationExtension : ObjectTypeExtension
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name(nameof(Mutation));
            descriptor.Field<UserResolver>(x => x.Register(default, default, default, default))
                .Argument("input", x => x.Type<RegisterUserInputType>());
            descriptor.Field<UserResolver>(x => x.Authenticate(default, default, default, default, default, default))
                .Argument("input", x => x.Type<AuthenticateInputType>());
            descriptor.Field<UserResolver>(x => x.AssignRoles(default, default, default))
                .Argument("input", x => x.Type<AssignRoleInputType>());
            base.Configure(descriptor);
        }
    }
}