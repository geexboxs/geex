using MongoDB.Entities;

namespace Geex.Core.Testing.Api.Aggregates.Tests
{
    public interface ITest : IEntity
    {
        string Name { get; set; }
        string Data { get; set; }
    }
}
