using Geex.Shared._ShouldMigrateToLib.Auth;
using Geex.Shared.Roots;
using HotChocolate.Types;
using HotChocolate.Types.Relay;

namespace Geex.Core.Users.GqlSchemas.Types.RootExtensions
{
    public class QueryExtension : ObjectTypeExtension
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name(nameof(Query));
            descriptor.Field<UserResolver>(x => x.QueryUsers(default, default))// return
                .UsePaging<UserType>()// paging
                .UseSorting<AppUser>(x =>
                {
                    x.BindFieldsExplicitly();
                    x.Sortable(y => y.Username);
                })// sort
                .UseFiltering<AppUser>(x =>
                {
                    x.BindFieldsExplicitly();
                    x.Filter(y => y.Username);
                })// filter
                ;
            base.Configure(descriptor);
        }
    }
}
