using HotChocolate.Types;

namespace Geex.Core.Users.Types
{
    public class UserType : ObjectType<User>
    {
        protected override void Configure(IObjectTypeDescriptor<User> descriptor)
        {
            base.Configure(descriptor);
        }
    }
}
