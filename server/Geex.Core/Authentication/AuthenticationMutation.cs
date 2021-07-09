using Autofac;

using Geex.Core.Users;
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

using Geex.Common.Abstractions;
using Geex.Common.Gql.Roots;
using Geex.Core.Authentication.Domain;
using Geex.Core.Authentication.GqlSchemas.Inputs;
using Geex.Shared._ShouldMigrateToLib;
using Geex.Shared._ShouldMigrateToLib.Auth;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Entities;

namespace Geex.Core.Authentication
{
    [ExtendObjectType(nameof(Mutation))]
    public class AuthenticationMutation : Mutation
    {
        public async Task<IdentityUserToken<string>> Authenticate([Parent] Mutation mutation,
            [Service] DbContext dbContext,
            [Service] UserTokenGenerateOptions userTokenGenerateOptions,
            AuthenticateInput input)
        {
            User? user;
            user = await dbContext.Find<User>().MatchUserIdentifier(input.UserIdentifier).ExecuteSingleAsync();
            if (user == default)
            {
                throw new BusinessException(GeexExceptionType.NotFound, message: "user not exists.");
            }
            return new UserToken(user, LoginProvider.Local, userTokenGenerateOptions);
        }
    }
}
