using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Geex.Shared._ShouldMigrateToLib.Abstractions;
using Geex.Shared._ShouldMigrateToLib.Json;

using JetBrains.Annotations;

using Microsoft.Extensions.Caching.Distributed;

namespace Geex.Shared._ShouldMigrateToLib
{
    public class GeexRedisClient : IGeexRedisClient
    {
        private IDistributedCache _redisClient;

        public GeexRedisClient(IDistributedCache redisClient)
        {
            _redisClient = redisClient;
        }

        public GeexRedisClient(RedisNamespace @namespace, IDistributedCache redisClient)
        {
            RedisNamespace = @namespace;
            _redisClient = redisClient;
        }

        public RedisNamespace RedisNamespace { get; }

        bool IGeexRedisClient.Disposed { get; set; }

        IDistributedCache IGeexRedisClient.RedisClient
        {
            get => _redisClient;
            set => _redisClient = value;
        }

        public DistributedCacheEntryOptions DefaultSetOptions { get; set; } = new();
    }

    public interface IGeexRedisClient : IDisposable
    {
        public RedisNamespace RedisNamespace { get; }
        public bool Disposed { get; protected set; }
        public IDistributedCache RedisClient { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="redisNamespace"></param>
        /// <returns></returns>
        IGeexRedisClient SwitchNamespace(RedisNamespace redisNamespace)
        {
            var redisClient = RedisClient;
            this.Dispose();
            var newInstance = Activator.CreateInstance(this.GetType(), args: new object?[] { redisNamespace, redisClient }) as IGeexRedisClient ?? throw new InvalidOperationException("IGeexRedisClient should have constructor like 'public GeexRedisClient(RedisNamespace @namespace, IDistributedCache redisClient)'");
            this.ApplyDefaultOptions();
            return newInstance;
        }

        void ApplyDefaultOptions()
        {
            if (this.RedisNamespace == RedisNamespace.Captcha)
            {
                this.DefaultSetOptions = new DistributedCacheEntryOptions()
                {
                    SlidingExpiration = TimeSpan.FromMinutes(10)
                };
            }
            else
            {
                this.DefaultSetOptions = new DistributedCacheEntryOptions();
            }
        }
        /// <summary>
        /// this will be the default option if no DistributedCacheEntryOptions when setting values
        /// </summary>
        DistributedCacheEntryOptions DefaultSetOptions { get; set; }

        void IDisposable.Dispose()
        {
            this.Disposed = true;
            this.RedisClient = null;
        }
        T Get<T>(string key)
        {
            return RedisClient.GetString(RedisNamespace == default ? key : $"{RedisNamespace}:{key}").ToObject<T>();
        }

        async Task<T> GetAsync<T>(string key, CancellationToken token = default(CancellationToken))
        {
            return (await RedisClient.GetStringAsync(RedisNamespace == default ? key : $"{RedisNamespace}:{key}", token)).ToObject<T>();
        }

        void Remove(string key)
        {
            RedisClient.Remove(RedisNamespace == default ? key : $"{RedisNamespace}:{key}");
        }

        async Task RemoveAsync(string key, CancellationToken token = default(CancellationToken))
        {
            await RedisClient.RemoveAsync(RedisNamespace == default ? key : $"{RedisNamespace}:{key}", token);
        }

        T GetAndRemove<T>(string key)
        {
            var result = this.Get<T>(key);
            this.Remove(key);
            return result;
        }

        async Task<T> GetAndRemoveAsync<T>(string key, CancellationToken token = default(CancellationToken))
        {
            var result = await this.GetAsync<T>(key, token);
            await this.RemoveAsync(key, token);
            return result;
        }

        void Set<T>(string key, T value)
        {
            RedisClient.SetString(RedisNamespace == default ? key : $"{RedisNamespace}:{key}", value.ToJson(), DefaultSetOptions);
        }
        void Set<T>(string key, T value, DistributedCacheEntryOptions options)
        {
            RedisClient.SetString(RedisNamespace == default ? key : $"{RedisNamespace}:{key}", value.ToJson(), options);
        }

        Task SetAsync<T>(
          string key,
          T value,
          DistributedCacheEntryOptions options,
          CancellationToken token = default(CancellationToken))
        {
            return RedisClient.SetStringAsync(RedisNamespace == default ? key : $"{RedisNamespace}:{key}", value.ToJson(), options, token);
        }
        Task SetAsync<T>(
          string key,
          T value,
          CancellationToken token = default(CancellationToken))
        {
            return RedisClient.SetStringAsync(RedisNamespace == default ? key : $"{RedisNamespace}:{key}", value.ToJson(), DefaultSetOptions, token);
        }
    }

    public class RedisNamespace : Enumeration<RedisNamespace, string>
    {
        public RedisNamespace([NotNull] string name, string value) : base(name, value)
        {
        }

        public RedisNamespace(string value) : base(value)
        {
        }
        public static readonly RedisNamespace Captcha = new RedisNamespace(nameof(Captcha));

    }
}
