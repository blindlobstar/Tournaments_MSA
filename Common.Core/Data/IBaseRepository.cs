using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Core.Data
{
    public interface IBaseRepository<TEntity, in TKey> where TEntity : class
    {
        Task<List<TEntity>> GetAll();
        Task<TEntity> Get(TKey id);
        void Update(TEntity entity);
        TEntity Add(TEntity entity);
        void Delete(TEntity entity);
        void Delete(TKey id);
    }
}