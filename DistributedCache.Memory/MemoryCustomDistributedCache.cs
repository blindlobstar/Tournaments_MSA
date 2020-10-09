using Common.CustomDistributedCache.Abstraction;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Common.DistributedCache.Memory
{
    public class MemoryCustomDistributedCache : MemoryDistributedCache, ICustomDistributedCache
    {
        public MemoryCustomDistributedCache(IOptions<MemoryDistributedCacheOptions> optionsAccessor) : base(optionsAccessor)
        {
        }

        public MemoryCustomDistributedCache(IOptions<MemoryDistributedCacheOptions> optionsAccessor, ILoggerFactory loggerFactory) : base(optionsAccessor, loggerFactory)
        {
        }

        public Task<TObject> GetObject<TObject>(string key)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> SetObject<TObject>(TObject @object)
        {
            throw new System.NotImplementedException();
        }
    }
}
