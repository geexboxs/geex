using HotChocolate.Types;

namespace Geex.Core.Users.GqlSchemas.Types
{
    public class UserType : ObjectType<AppUser>
    {
        protected override void Configure(IObjectTypeDescriptor<AppUser> descriptor)
        {
            base.Configure(descriptor);
        }
    }
}
