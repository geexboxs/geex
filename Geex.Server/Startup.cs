using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using Geex.Core;
using Geex.Shared;
using Geex.Shared._ShouldMigrateToLib;
using Geex.Shared._ShouldMigrateToLib.Auth;
using HotChocolate;
using Humanizer;
using IdentityModel;
using IdentityServer4.MongoDB.Mappers;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;

namespace Geex.Server
{
    public class Startup
    {
        public Startup()
        {

        }

        //// This is the default if you don't have an environment specific method.
        public void ConfigureServices(IServiceCollection services)
        {
        }

        //// This is the default if you don't have an environment specific method.
        //public void ConfigureContainer(ContainerBuilder builder)
        //{
        //}

        // This is the default if you don't have an environment specific method.
        public void Configure(IApplicationBuilder app)
        {
            app.InitializeApplication();
        }
    }
}