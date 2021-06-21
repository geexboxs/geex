using Geex.Common.Abstractions;
using StackExchange.Redis.Extensions.Core.Configuration;

namespace Geex.Common.Redis
{
    public class RedisModuleOptions : GeexModuleOption<RedisModule>
    {
        public RedisConfiguration Redis { get; set; }
    }
}