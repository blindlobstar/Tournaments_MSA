using Common.Core.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Common.Data.MongoDB.Repositories
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity, string>
        where TEntity : class, IEntity<string>, new()
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
            var filter = Builders<TEntity>.Filter.Eq("_id", ObjectId.Parse(entity.Id));
            Collection.DeleteOne(filter);
        }

        public virtual void Delete(string id)
        {
            var filter = Builders<TEntity>.Filter.Eq("_id", ObjectId.Parse(id));
            Collection.DeleteOne(filter);
        }

        public virtual Task<List<TEntity>> GetAll() =>
            Collection.FindSync(Builders<TEntity>.Filter.Empty).ToListAsync();

        public virtual Task<TEntity> Get(string id)
        {
            var bsonId = ObjectId.TryParse(id, out var t) ? t : ObjectId.Empty;
            if(bsonId == ObjectId.Empty)
            {
                return null;
            }
            var filter = Builders<TEntity>.Filter.Eq("_id", ObjectId.Parse(id));
            return Collection.FindSync(filter).FirstOrDefaultAsync();
        }


        public virtual void Update(TEntity entity)
        {
            var filter = Builders<TEntity>.Filter.Eq("_id", ObjectId.Parse(entity.Id));
            Collection.ReplaceOne(filter, entity);
        }
    }
}