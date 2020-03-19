using Common.Data.MongoDB.Models;
using MongoDB.Driver;

namespace Common.Data.MongoDB
{
    public interface IBaseContext<TEntity> where TEntity : class
    {
        IDatabaseSettings DatabaseSettings { get; }
        IMongoCollection<TEntity> GetCollection(string name);
    }
}