using Geex.Common.Abstractions;
using StackExchange.Redis.Extensions.Core.Configuration;

namespace Geex.Common.Redis
{
    public class RedisModuleOptions : IGeexModuleOption<RedisModule>
    {
        public RedisConfiguration Redis { get; set; }
    }
}