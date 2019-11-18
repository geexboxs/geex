using HotChocolate.Types;

namespace Geex.Core.UserManagement.Types.Inputs
{
    public class AssignRoleInputType : InputObjectType<AssignRoleInput>
    {

        protected override void Configure(IInputObjectTypeDescriptor<AssignRoleInput> descriptor)
        {
            descriptor.Field(t => t.UserId).Type<NonNullType<IdType>>();
            descriptor.Field(t => t.Roles).Type<NonNullType<ListType<IdType>>>();
        }
    }
}
