using Common.Core.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public virtual void Delete(TEntity entity)
        {
            var filter = Builders<TEntity>.Filter.Eq("_id", entity.Id);
            Collection.DeleteOne(filter);
        }

        public virtual void Delete(TKey id)
        {
            var filter = Builders<TEntity>.Filter.Eq("_id", id);
            Collection.DeleteOne(filter);
        }

        public virtual Task<List<TEntity>> GetAll() =>
            Collection.FindSync(Builders<TEntity>.Filter.Empty).ToListAsync();

        public virtual Task<TEntity> Get(TKey id)
        {
            var filter = Builders<TEntity>.Filter.Eq("_id", id);
            return Collection.FindSync(filter).FirstOrDefaultAsync();
        }


        public virtual void Update(TEntity entity)
        {
            var filter = Builders<TEntity>.Filter.Eq("_id", entity.Id);
            Collection.ReplaceOne(filter, entity);
        }
    }
}