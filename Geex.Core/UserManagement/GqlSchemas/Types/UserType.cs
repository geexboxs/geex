using Geex.Core.Authentication.GqlSchemas.Types;
using Geex.Shared._ShouldMigrateToLib.Auth;

using HotChocolate.Types;

namespace Geex.Core.UserManagement.GqlSchemas.Types
{
    public class UserType : ObjectType<User>
    {
        protected override void Configure(IObjectTypeDescriptor<User> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            //descriptor.Field(x => x.Username);
            //descriptor.Field(x => x.Claims);
            //descriptor.Field(x => x.Email);
            //descriptor.Field(x => x.PhoneNumber);
            //descriptor.Field(x => x.Roles);
            //descriptor.Field(x => x.ID);
            //descriptor.Ignore(x => x.Claims);
            //descriptor.Ignore(x => x.AuthorizedPermissions);
            descriptor.Field(x => x.Roles).Type<ListType<RoleType>>().Resolve(x => x.ToString());
            base.Configure(descriptor);
        }
    }
}
