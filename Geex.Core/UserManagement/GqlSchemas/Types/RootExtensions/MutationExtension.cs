using Geex.Core.UserManagement.GqlSchemas.Types.Inputs;
using Geex.Shared.Roots;

using HotChocolate.Types;

namespace Geex.Core.UserManagement.GqlSchemas.Types.RootExtensions
{
    public class MutationExtension : ObjectTypeExtension
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name(nameof(Mutation));
            descriptor.Field<RoleResolver>(x => x.CreateRole(default, default, default))
                .Argument("input", x => x.Type<CreateRoleInputType>());
            descriptor.Field<UserResolver>(x => x.Register(default, default, default))
                .Argument("input", x => x.Type<RegisterUserInputType>());
            descriptor.Field<UserResolver>(x => x.AssignRoles(default, default, default))
                .Argument("input", x => x.Type<AssignRoleInputType>()).Authorize("Permission");
            descriptor.Field<UserResolver>(x => x.AssignRoles(default, default, default))
                .Argument("input", x => x.Type<AssignRoleInputType>()).Authorize("Permission");
            base.Configure(descriptor);
        }
    }
}