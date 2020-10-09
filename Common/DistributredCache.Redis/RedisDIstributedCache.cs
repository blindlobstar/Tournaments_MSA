using Common.CustomDistributedCache.Abstraction;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace DistributredCache.Redis
{
    public class RedisDIstributedCache : RedisCache, ICustomDistributedCache
    {
        public RedisDIstributedCache(IOptions<RedisCacheOptions> optionsAccessor) : base(optionsAccessor) { }

        public async Task<TObject> GetObject<TObject>(string key)
        {
            var jsonData = await this.GetStringAsync(key);

            return JsonConvert.DeserializeObject<TObject>(jsonData);
        }

        public async Task<string> SetObject<TObject>(TObject @object)
        {
            var g = Guid.NewGuid();
            var key = Convert.ToBase64String(g.ToByteArray());
            key = key.Replace("=", "").Replace("+", "");

            var jsonData = JsonConvert.SerializeObject(@object);

            await this.SetStringAsync(key, jsonData);
            
            return key;
        }
    }
}
