using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geex.Shared.Roots;
using HotChocolate.Types;
using HotChocolate.Types.Relay;

namespace Geex.Core.User.Types.RootExtensions
{
    public class QueryExtension : ObjectTypeExtension
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name(nameof(Query));
            descriptor.Field<UserResolver>(x => x.QueryUsers(default, default))// return
                .UsePaging<UserType>()// paging
                .UseSorting<User>(x =>
                {
                    x.BindFieldsExplicitly();
                    x.Sortable(y => y.Id);
                })// sort
                .UseFiltering<User>(x =>
                {
                    x.BindFieldsExplicitly();
                    x.Filter(y => y.Id);
                })// filter
                ;
            base.Configure(descriptor);
        }
    }
}
