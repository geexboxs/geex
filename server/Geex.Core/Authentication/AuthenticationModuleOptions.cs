using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geex.Common.Abstractions;
using Volo.Abp.DependencyInjection;

namespace Geex.Core.Authentication
{
    public class AuthenticationModuleOptions:IGeexModuleOption<AuthenticationModule>
    {
        public string ValidIssuer { get; set; } = "Geex";
        public string ValidAudience { get; set; } = "Geex";
        public string SecurityKey { get; set; } = "Geex";
    }
}
