using Common.Core.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Data.MongoDB.Models;
using MongoDB.Driver;

namespace Common.Data.MongoDB.Repositories
{
    public abstract class BaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>, new()
    {
        protected IMongoCollection<TEntity> Collection { get; set; }

        protected BaseRepository(IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            Collection = database.GetCollection<TEntity>(databaseSettings.CollectionName);
        }

        public TEntity Add(TEntity entity)
        {
            Collection.InsertOne(entity);
            return entity;
        }

        public void Delete(TEntity entity) =>
            Collection.DeleteOne(e => e.Id.Equals(entity.Id));

        public void Delete(TKey id) =>
            Collection.DeleteOne(e => e.Id.Equals(id));

        public async Task<List<TEntity>> Get() =>
            await Collection.Find(e => true).ToListAsync();

        public async Task<TEntity> Get(TKey id) =>
            await Collection.Find(e => e.Id.Equals(id)).FirstOrDefaultAsync();

        public void Update(TEntity entity) =>
            Collection.ReplaceOne(e => e.Id.Equals(entity.Id), entity);
    }
}