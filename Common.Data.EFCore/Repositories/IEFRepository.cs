using Common.Core.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Data.EFCore.Repositories
{
    public interface IEFRepository<TEntity, in TKey> : IBaseRepository<TEntity, TKey> 
        where TEntity : class
    {
        Task<List<TEntity>> GetAll(string include);
        Task<List<TEntity>> GetAll(IEnumerable<string> includes);
        Task<TEntity> Get(TKey id, string include);
        Task<TEntity> Get(TKey id, IEnumerable<string> includes);
        Task SaveChanges();
    }
}