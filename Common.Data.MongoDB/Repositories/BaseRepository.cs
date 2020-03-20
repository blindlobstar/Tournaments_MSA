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
        protected IMongoCollection<TEntity> Collection { get; }

        protected BaseRepository(IBaseContext<TEntity> context)
        {
            Collection = context.GetCollection(context.DatabaseSettings.CollectionName);
        }

        public virtual TEntity Add(TEntity entity)
        {
            Collection.InsertOne(entity);
            return entity;
        }

        public virtual void Delete(TEntity entity) =>
            Collection.DeleteOne(e => e.Id.Equals(entity.Id));

        public virtual void Delete(TKey id) =>
            Collection.DeleteOne(e => e.Id.Equals(id));

        public virtual async Task<List<TEntity>> Get() =>
            await Collection.FindSync(Builders<TEntity>.Filter.Empty,null).ToListAsync();

        public virtual async Task<TEntity> Get(TKey id)
        {
            FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq("_id", id);
            return await Collection.FindSync(filter).FirstOrDefaultAsync();
        }
            

        public virtual void Update(TEntity entity) =>
            Collection.ReplaceOne(e => e.Id.Equals(entity.Id), entity);
    }
}