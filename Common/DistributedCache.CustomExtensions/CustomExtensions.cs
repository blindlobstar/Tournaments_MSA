using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Common.DistributedCache.CustomExtensions
{
    public static class CustomExtensions
    {
        public static async Task<string> SetEntity(this IDistributedCache distributedCache, object entity)
        {
            //Generating unique string key
            var g = Guid.NewGuid();
            var key = Convert.ToBase64String(g.ToByteArray());
            key = key.Replace("=", "").Replace("+", "");

            var jsonData = JsonConvert.SerializeObject(entity);

            await distributedCache.SetStringAsync(key, jsonData);

            return key;
        }

        public static async Task<TEntity> GetEntity<TEntity>(this IDistributedCache distributedCache, string key)
        {
            var jsonData = await distributedCache.GetStringAsync(key);
            if (string.IsNullOrEmpty(jsonData))
                return default;

            return JsonConvert.DeserializeObject<TEntity>(jsonData);
        }
    }
}
