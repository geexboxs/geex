using System;
using System.Collections.Generic;
using System.Text;
using HotChocolate.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Modularity;

namespace Geex.Shared._ShouldMigrateToLib
{
    public class FrameworkFixModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            context.Services.TryAddSingleton(x=>x.GetService<IHostingEnvironment>() as IWebHostEnvironment);
#pragma warning restore CS0618 // Type or member is obsolete
            base.PreConfigureServices(context);
        }
    }
}
