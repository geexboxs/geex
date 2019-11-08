using Geex.Core.User.Types.Inputs;
using Geex.Shared.Roots;
using HotChocolate.Types;

namespace Geex.Core.User.Types.RootExtensions
{
    public class MutationExtension : ObjectTypeExtension
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name(nameof(Mutation));
            descriptor.Field<UserResolver>(x => x.Register(default, default, default)).Argument("input", x => x.Type<RegisterUserInputType>());
            base.Configure(descriptor);
        }
    }
}