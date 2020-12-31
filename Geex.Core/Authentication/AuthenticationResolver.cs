using Autofac;

using Geex.Core.Users;
using Geex.Shared.Roots;

using HotChocolate;
using HotChocolate.Types;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;

using MongoDB.Driver;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Geex.Core.Authentication.Domain;
using Geex.Core.Authentication.GqlSchemas.Inputs;
using Geex.Shared._ShouldMigrateToLib;
using Geex.Shared._ShouldMigrateToLib.Abstractions;
using Geex.Shared._ShouldMigrateToLib.Auth;
using Microsoft.Extensions.Configuration;
using MongoDB.Entities;

namespace Geex.Core.Authentication
{
    [GraphQLResolverOf(typeof(StringType))]
    [GraphQLResolverOf(typeof(Query))]
    public class AuthenticationResolver
    {
        public async Task<IdentityUserToken<string>> Authenticate([Parent] Mutation mutation,
            [Service] IComponentContext componentContext,
            AuthenticateInput input)
        {
            IConfiguration configuration = componentContext.Resolve<IConfiguration>();
            var user = await User.FindAsync(input.UserIdentifier, CancellationToken.None);
            return new UserToken(user, LoginProvider.Local, componentContext.Resolve<UserTokenGenerateOptions>());
            //return new UserToken(user, LoginProvider.Local, default);
        }
    }
}
