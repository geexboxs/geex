using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geex.Common.Abstractions
{
    public interface IHasStartupInitialize
    {
        Task Initialize();
    }
}
