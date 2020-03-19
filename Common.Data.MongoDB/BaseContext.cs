using Common.Data.MongoDB.Models;
using MongoDB.Driver;

namespace Common.Data.MongoDB
{
    public abstract class BaseContext<TEntity> : IBaseContext<TEntity>
        where TEntity : class
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoClient _mongoClient;
        public IDatabaseSettings DatabaseSettings { get; protected set; }

        protected BaseContext(IDatabaseSettings databaseSettings)
        {
            DatabaseSettings = databaseSettings;
            _mongoClient = new MongoClient(databaseSettings.ConnectionString);
            _database = _mongoClient.GetDatabase(databaseSettings.DatabaseName);
        }

        public IMongoCollection<TEntity> GetCollection(string name) =>
            _database.GetCollection<TEntity>(name);
    }
}