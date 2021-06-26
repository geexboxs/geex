using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Geex.Core.Testing.Api.Aggregates.Tests;
using Geex.Core.Testing.Api.GqlSchemas.Tests.Inputs;
using Geex.Core.Testing.Core.Aggregates.Tests;
using MediatR;
using MongoDB.Entities;

namespace Geex.Core.Testing.Core.Handlers
{
    /// <summary>
    /// 系统参数化, XXXHandler等价于XXXManager
    /// 相比XXXManager, XXXHandler是一系列Request的组合
    /// XXXHandler是逻辑的处理单元, 其和接口函数一一对应, 但是它不处理授权等管道逻辑
    /// </summary>
    public class TestHandler : IRequestHandler<GetTestsInput, IEnumerable<ITest>>, IRequestHandler<UpdateTestInput, ITest>
    {
        public DbContext DbContext { get; }

        public TestHandler(DbContext dbContext)
        {
            DbContext = dbContext;
        }
        public async Task<IEnumerable<ITest>> Handle(GetTestsInput input, CancellationToken cancellationToken)
        {
            return await DbContext.Find<Test>().Match(x => x.Name == input.Name).ExecuteAsync(cancellationToken);
        }

        public async Task<ITest> Handle(UpdateTestInput input, CancellationToken cancellationToken)
        {
            var entity = await DbContext.Find<Test>().Match(x => x.Name == input.Name).ExecuteFirstAsync(cancellationToken);
            entity.Name = input.NewName;
            return entity;
        }
    }
}
