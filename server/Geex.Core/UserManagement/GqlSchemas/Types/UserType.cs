using Geex.Core.Authentication.Domain;
using Geex.Core.Authentication.GqlSchemas.Types;
using Geex.Core.UserManagement.Domain;
using Geex.Shared._ShouldMigrateToLib.Auth;

using HotChocolate.Types;

namespace Geex.Core.UserManagement.GqlSchemas.Types
{
    public class UserType : ObjectType<User>
    {
        protected override void Configure(IObjectTypeDescriptor<User> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            //descriptor.Field(x => x.UserName);
            //descriptor.Field(x => x.Claims);
            //descriptor.Field(x => x.Email);
            //descriptor.Field(x => x.PhoneNumber);
            //descriptor.Field(x => x.Roles);
            descriptor.Field(x => x.Id);
            //descriptor.Ignore(x => x.Claims);
            //descriptor.Ignore(x => x.AuthorizedPermissions);
            descriptor.Field(x => x.Roles).Type<ListType<RoleType>>().Resolve(x => x.ToString());
            base.Configure(descriptor);
        }
    }

    public class UserProfileType : ObjectType<IUserProfile>
    {
        protected override void Configure(IObjectTypeDescriptor<IUserProfile> descriptor)
        {
            descriptor.BindFieldsImplicitly();
            base.Configure(descriptor);
        }
    }
}
