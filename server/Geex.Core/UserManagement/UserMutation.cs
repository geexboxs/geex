﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Autofac;

using Geex.Core.Authentication.Domain;
using Geex.Core.UserManagement.Domain;
using Geex.Core.UserManagement.GqlSchemas.Inputs;
using Geex.Core.Users;
using Geex.Shared._ShouldMigrateToLib;
using Geex.Shared._ShouldMigrateToLib.Auth;
using Geex.Shared.Roots;

using HotChocolate;
using HotChocolate.Types;

using Microsoft.AspNetCore.Identity;

using MongoDB.Entities;

namespace Geex.Core.UserManagement
{
    [ExtendObjectType(nameof(Mutation))]
    public class UserMutation : Mutation
    {
        public async Task<bool> Register([Parent] Mutation mutation,
            [Service] IUserCreationValidator userCreationValidator,
            [Service] IPasswordHasher<User> passwordHasher,
            RegisterUserInput input)
        {
            var user = new User(userCreationValidator, passwordHasher, input.PhoneOrEmail, input.Password, input.UserName);
            await user.SaveAsync();
            return true;
        }

        public async Task<bool> AssignRoles([Parent] Mutation mutation, [Service] DbContext dbContext, AssignRoleInput input)
        {
            var user = await dbContext.Find<User>().OneAsync(input.UserId.ToString());
            var roles = await dbContext.Find<Role>().ManyAsync(x => input.Roles.Contains(x.Id));
            await user.AssignRoles(roles);
            return true;
        }

        public async Task<bool> UpdateProfile([Parent] Mutation mutation, [Service] DbContext dbContext, UploadProfileInput input)
        {
            var user = await dbContext.Find<User>().OneAsync(input.UserId.ToString());
            if (!input.NewAvatar.IsNullOrEmpty())
            {
                var userAvatarClaim = user.Claims.GetOrAdd(x => x.ClaimType == GeexClaimType.Avatar, () => new UserClaim(GeexClaimType.Avatar, input.NewAvatar));
            }
            await user.SaveAsync();
            return true;
        }

    }
}