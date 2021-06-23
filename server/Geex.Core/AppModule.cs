using System.Threading.Tasks;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Volo.Abp;
using Volo.Abp.Modularity;
using Autofac.Extensions.DependencyInjection;
using Geex.Common;
using Geex.Common.Abstractions;
using Geex.Common.Settings;
using Geex.Common.Settings.Api;
using Geex.Core.Authentication;
using Geex.Core.Authorization;
using Geex.Core.Captcha;
using Geex.Core.Localization;
using Geex.Core.Notification;
using Geex.Core.Settings;
using Geex.Core.Testing.Api;
using Geex.Core.UserManagement;
using Microsoft.AspNetCore.Http;
using Geex.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Geex.Core.Users;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MongoDB.Entities;
using StackExchange.Redis.Extensions.Core;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Validation;

namespace Geex.Core
{
    [DependsOn(
        typeof(GeexCommonModule),
        typeof(AuthenticationModule),
        typeof(NotificationModule),
        typeof(AuthorizationModule),
        typeof(TestingModule),
        typeof(CaptchaModule),
        typeof(SettingsModule),
        typeof(LocalizationModule),
        typeof(UserManagementModule)
        )]
    public class AppModule : GeexEntryModule<AppModule>
    {
        private IWebHostEnvironment _env;
        private IConfiguration _configuration;

        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            base.PreConfigureServices(context);
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            base.ConfigureServices(context);
        }



        public override void OnApplicationInitialization(
            ApplicationInitializationContext context)
        {
            base.OnApplicationInitialization(context);
        }
    }
}
