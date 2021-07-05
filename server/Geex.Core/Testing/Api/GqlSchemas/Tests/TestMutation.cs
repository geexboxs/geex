﻿using System.Threading.Tasks;
using Geex.Common.Gql.Roots;
using Geex.Core.Testing.Api.Aggregates.Tests;
using Geex.Core.Testing.Api.GqlSchemas.Tests.Inputs;
using HotChocolate.Types;

namespace Geex.Core.Testing.Api.GqlSchemas.Tests
{
    [ExtendObjectType(nameof(Mutation))]
    public class TestMutation : Mutation
    {
        /// <summary>
        /// 更新设置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ITest> UpdateTest(
            UpdateTestInput input)
        {
            var result = await Mediator.Send(input);
            return result;
        }
    }
}