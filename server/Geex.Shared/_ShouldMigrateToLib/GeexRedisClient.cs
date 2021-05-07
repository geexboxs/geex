//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using Geex.Shared._ShouldMigrateToLib.Json;
//using Microsoft.Extensions.Caching.Distributed;

//using ServiceStack.Redis;

//namespace Geex.Shared._ShouldMigrateToLib
//{
//    public class GeexRedisClient : IGeexRedisClient
//    {
//        private IRedisClientAsync _redisClient;

//        public GeexRedisClient(IRedisClientAsync redisClient)
//        {
//            this._redisClient = redisClient;
//        }

//        public GeexRedisClient(RedisNamespace @namespace, IRedisClientAsync redisClient)
//        {
//            this.RedisNamespace = @namespace;
//            this._redisClient = redisClient;
//        }

//        public RedisNamespace RedisNamespace { get; }

//        bool IGeexRedisClient.Disposed { get; set; }

//        IRedisClientAsync IGeexRedisClient.RedisClient
//        {
//            get => this._redisClient;
//            set => this._redisClient = value;
//        }

//        public TimeSpan? DefaultSlidingExpiration { get; set; }
//    }

//    public interface IGeexRedisClient : IDisposable
//    {
//        public RedisNamespace RedisNamespace { get; }
//        public bool Disposed { get; protected set; }
//        public IRedisClientAsync RedisClient { get; protected set; }
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="redisNamespace"></param>
//        /// <returns></returns>
//        IGeexRedisClient SwitchNamespace(RedisNamespace redisNamespace)
//        {
//            var redisClient = this.RedisClient;
//            this.Dispose();
//            var newInstance = Activator.CreateInstance(this.GetType(), args: new object?[] { redisNamespace, redisClient }) as IGeexRedisClient ?? throw new InvalidOperationException("IGeexRedisClient should have constructor like 'public GeexRedisClient(RedisNamespace @namespace, IDistributedCache redisClient)'");
//            this.ApplyDefaultOptions();
//            return newInstance;
//        }

//        void ApplyDefaultOptions()
//        {
//            if (this.RedisNamespace == RedisNamespace.Captcha)
//            {
//                this.DefaultSlidingExpiration = TimeSpan.FromMinutes(10);
//            }
//        }
//        /// <summary>
//        /// this will be the default option if no DistributedCacheEntryOptions when setting values
//        /// </summary>
//        TimeSpan? DefaultSlidingExpiration { get; set; }

//        void IDisposable.Dispose()
//        {
//            this.Disposed = true;
//            this.RedisClient = null;
//        }
//        T Get<T>(string key)
//        {
//            return this.GetAsync<T>(key).Result;
//        }

//        async Task<T> GetAsync<T>(string key, CancellationToken token = default(CancellationToken))
//        {
//            return (await this.RedisClient.GetAsync<T>(this.RedisNamespace == default ? key : $"{this.RedisNamespace}:{key}", token));
//        }

//        bool Remove(string key)
//        {
//            return this.RemoveAsync(key).Result;
//        }
//        bool Clear()
//        {
//            return this.RedisClient.RemoveAsync(this.RedisNamespace).Result;
//        }

//        async Task<bool> RemoveAsync(string key, CancellationToken token = default(CancellationToken))
//        {
//            this.ValidateKey(key);
//            return await this.RedisClient.RemoveAsync(this.RedisNamespace == default ? key : $"{this.RedisNamespace}:{key}", token);
//        }

//        void ValidateKey(string key)
//        {
//            if (key.IsNullOrEmpty())
//            {
//                throw new InvalidOperationException();
//            }
//        }

//        T GetAndRemove<T>(string key)
//        {
//            return this.GetAndRemoveAsync<T>(key).Result;
//        }

//        async Task<T> GetAndRemoveAsync<T>(string key, CancellationToken token = default(CancellationToken))
//        {
//            this.ValidateKey(key);
//            var result = await this.GetAsync<T>(key, token);
//            await this.RemoveAsync(key, token);
//            return result;
//        }

//        bool Set<T>(string key, T value)
//        {
//            return this.SetAsync(key, value).Result;
//        }
//        bool Set<T>(string key, T value, TimeSpan expireIn)
//        {
//            return this.SetAsync(key, value, expireIn).Result;
//        }

//        async Task<bool> SetAsync<T>(
//          string key,
//          T value,
//          TimeSpan expireIn,
//          CancellationToken token = default(CancellationToken))
//        {
//            this.ValidateKey(key);
//            return await this.RedisClient.AddAsync(this.RedisNamespace == default ? key : $"{this.RedisNamespace}:{key}", value, expireIn, token);
//        }

//        async Task<bool> SetAsync<T>(
//          string key,
//          T value,
//          CancellationToken token = default(CancellationToken))
//        {
//            this.ValidateKey(key);
//            if (this.DefaultSlidingExpiration == null)
//            {
//                return await this.RedisClient.AddAsync(this.RedisNamespace == default ? key : $"{this.RedisNamespace}:{key}", value, token);
//            }
//            else
//            {
//                return await this.RedisClient.AddAsync(this.RedisNamespace == default ? key : $"{this.RedisNamespace}:{key}", value, this.DefaultSlidingExpiration.Value, token);
//            }
//        }
//    }
//}
