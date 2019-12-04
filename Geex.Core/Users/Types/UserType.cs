using HotChocolate.Types;

namespace Geex.Core.Users.Types
{
    public class UserType : ObjectType<AppUser>
    {
        protected override void Configure(IObjectTypeDescriptor<AppUser> descriptor)
        {
            base.Configure(descriptor);
        }
    }
}
