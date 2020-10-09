using Microsoft.Extensions.Caching.Distributed;
using System.Threading.Tasks;

namespace Common.CustomDistributedCache.Abstraction
{
    public interface ICustomDistributedCache : IDistributedCache
    {
        Task<string> SetObject<TObject>(TObject @object);
        Task<TObject> GetObject<TObject>(string key);
    }
}
