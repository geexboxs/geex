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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using ApiResource = IdentityServer4.MongoDB.Entities.ApiResource;
using Client = IdentityServer4.MongoDB.Entities.Client;
using IdentityResource = IdentityServer4.MongoDB.Entities.IdentityResource;

namespace Geex.Server
{
    public class Startup
    {
        private readonly IHostEnvironment _env;

        public Startup(IHostEnvironment env)
        {
            _env = env;
            // In ASP.NET Core 3.0 `env` will be an IWebHostingEnvironment, not IHostingEnvironment.
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }


        // This is the default if you don't have an environment specific method.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCasbinAuthorization();
            services.AddScoped<GeexCookieAuthenticationEvents>();
            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.EventsType = typeof(GeexCookieAuthenticationEvents);
                })
                    .AddIdentityServerAuthentication(
                        options =>
                        {
                            if (_env.IsDevelopment())
                            {
                                IdentityModelEventSource.ShowPII = true;
                            }
                            options.ApiName = _env.ApplicationName;
                            options.Authority = $"http://{Environment.GetEnvironmentVariable("HOST_NAME")}";
                            options.ApiSecret = _env.ApplicationName;
                            options.RequireHttpsMetadata = false;
                            options.JwtBearerEvents = new JwtBearerEvents
                            {
                                OnChallenge = context =>
                                {
                                    return Task.CompletedTask;
                                },
                                OnTokenValidated = ContextBoundObject =>
                                {
                                    return Task.CompletedTask;
                                },
                                OnMessageReceived = context =>
                                {
                                    return Task.CompletedTask;
                                },
                                OnAuthenticationFailed = context =>
                                {
                                    var te = context.Exception;
                                    return Task.CompletedTask;
                                }
                            };
                        });

            services.AddHealthChecks();

            AddDefaultIdentityServer(services);
        }



        // This is the default if you don't have an environment specific method.
        public void ConfigureContainer(ContainerBuilder builder)
        {

            builder.AddGeexGraphQL<AppModule>();
            builder.Register(x => new PasswordHasher<AuthUser>()).AsImplementedInterfaces();
            AddMongoDb(builder, this.Configuration.GetConnectionString("Default"));
        }



        public ISchemaBuilder SchemaBuilder { get; set; }

        private static void AddMongoDb(ContainerBuilder builder, string connectionString)
        {
            var pack = new ConventionPack();
            pack.Add(new IgnoreExtraElementsConvention(true));
            ConventionRegistry.Register("My Solution Conventions", pack, t => true);
            builder.Register(x => new MongoUrl(connectionString)).AsSelf().AsImplementedInterfaces();
            builder.Register(x => new MongoClient(x.Resolve<MongoUrl>())).AsSelf().AsImplementedInterfaces();
            builder.Register(x => x.Resolve<IMongoClient>().GetDatabase(x.Resolve<MongoUrl>().DatabaseName)).AsSelf()
                .AsImplementedInterfaces();
            builder.RegisterSource<MongoRepositoryRegistrationSource>();
        }

        // This is the default if you don't have an environment specific method.
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(app.ApplicationServices.GetAutofacRoot()));

            InitializeIdentityServerDatabase(app);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
            });

            app.UseHealthChecks("/health-check");

            //app.UseAuthentication();
            app.UseIdentityServer();

            app.UseGeexGraphQL();
        }

        private void AddDefaultIdentityServer(IServiceCollection services)
        {
            services.AddIdentityServer(options =>
                {
                    options.Authentication.CookieLifetime = new TimeSpan(24, 0, 0);
                    options.Authentication.CookieSlidingExpiration = false;
                })
                .AddJwtBearerClientAuthentication()
                .AddDeveloperSigningCredential()
                .AddResourceOwnerValidator<GeexPasswordValidator>()
                // lulus:此处添加正式证书,不知道证书加密类型,未作处理
                //.AddSigningCredential()
                .AddMongoRepository(this.Configuration.GetConnectionString("Default"))
                .AddRedirectUriValidator<RegexRedirectUriValidator>()
                .AddCorsPolicyService<RegexCorsPolicyService>();
            services.AddSingleton<IdentityServerSeedConfig>();
            //bug :idsr的bug
            //services.Replace(ServiceDescriptor.Transient<ICorsPolicyService, RegexCorsPolicyService>());
        }

        private static void InitializeIdentityServerDatabase(IApplicationBuilder app)
        {

            bool createdNewRepository = false;
            var repository = app.ApplicationServices.GetService<IMongoDatabase>().GetCollection<Client>(nameof(Client).Humanize().Pluralize());
            if (repository.AsQueryable().Any())
            {
                return;
            }

            //  --Client
            IdentityServerSeedConfig identityServerSeedConfig = app.ApplicationServices.GetService<IdentityServerSeedConfig>();
            foreach (var client in identityServerSeedConfig.Clients)
            {
                app.ApplicationServices.GetService<IMongoDatabase>().GetCollection<Client>(nameof(Client).Humanize().Pluralize()).InsertOne(client.ToEntity());
            }
            foreach (var res in identityServerSeedConfig.IdentityResources)
            {
                app.ApplicationServices.GetService<IMongoDatabase>().GetCollection<IdentityResource>(nameof(IdentityResource).Humanize().Pluralize()).InsertOne(res.ToEntity());
            }
            foreach (var api in identityServerSeedConfig.ApiResources)
            {
                app.ApplicationServices.GetService<IMongoDatabase>().GetCollection<ApiResource>(nameof(ApiResource).Humanize().Pluralize()).InsertOne((api.ToEntity()));
            }
            createdNewRepository = true;


            // If it's a new Repository (database), need to restart the website to configure Mongo to ignore Extra Elements.
            if (createdNewRepository)
            {
                var newRepositoryMsg = $"Mongo Repository created/populated! Please restart you website, so Mongo driver will be configured  to ignore Extra Elements - e.g. IdentityServer \"_id\" ";
                throw new Exception(newRepositoryMsg);
            }
        }
    }
}