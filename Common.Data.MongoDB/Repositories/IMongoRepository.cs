using Common.Core.Data;
using System.Threading.Tasks;

namespace Common.Data.MongoDB.Repositories
{
    public interface IMongoRepository<TEntity> : IBaseRepository<TEntity, string> where TEntity : class
    {
        Task UpdateAsync(TEntity entity);
    }
}
