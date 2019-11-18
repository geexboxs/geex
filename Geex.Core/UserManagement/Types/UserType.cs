using HotChocolate.Types;

namespace Geex.Core.UserManagement.Types
{
    public class UserType : ObjectType<User>
    {
        protected override void Configure(IObjectTypeDescriptor<User> descriptor)
        {
            base.Configure(descriptor);
        }
    }
}
