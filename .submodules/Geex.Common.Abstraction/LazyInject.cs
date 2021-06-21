using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Volo.Abp.DependencyInjection;

namespace Geex.Common.Abstractions
{
    public class LazyInject<T> : Lazy<T>, ITransientDependency
    {
        public LazyInject(IServiceProvider provider)
        : base(() => provider.GetRequiredService<T>())
        {

        }
    }
}
