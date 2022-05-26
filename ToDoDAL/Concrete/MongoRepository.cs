using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoDomainModels.Model.Mongo;
using ToDoDomainModels.Repositories;

namespace ToDoDAL.Concrete
{
    public class MongoRepository<T> : IMongoRepository<T> where T : class, IEntity
    {
        private readonly MongoClient _mongoClient;
        private readonly IMongoDatabase _mongoDatabase;
        private readonly IMongoCollection<T> _collection;

        public MongoRepository()
        {
            _mongoClient = new MongoClient("mongodb://192.168.1.144:27017");
            _mongoDatabase = _mongoClient.GetDatabase("todo");
            _collection = _mongoDatabase.GetCollection<T>(GetCollectionNameFromType(typeof(T)));
        }

        private string GetCollectionNameFromType(Type entitytype)
        {
            return entitytype.Name.ToLower();
        }

        public IEnumerable<T> GetList()
        {
            return _collection.AsQueryable().ToList();
        }

        public async Task<IEnumerable<T>> GetListAsync()
        {
            return await _collection.AsQueryable().ToListAsync();
        }

        public T GetItem(string id)
        {
            try
            {
                return _collection.AsQueryable().SingleOrDefault(x => x.Id == id);
            }
            catch (Exception)
            {
                return null;
            }

        }

        public async Task<T> GetItemAsync(string id)
        {
            try
            {
                return await _collection.AsQueryable().Where(x => x.Id == id).SingleOrDefaultAsync();
            }
            catch (Exception)
            {
                return null;
            }

        }

        public T Create(T item)
        {
            _collection.InsertOne(item);
            return item;
        }

        public async Task<T> CreateAsync(T item)
        {
            await _collection.InsertOneAsync(item);
            return item;
        }

        public void Delete(string id)
        {
            var filter = new BsonDocument("_id", new BsonObjectId(new ObjectId(id)));
            _collection.DeleteOne(filter);
        }

        public async System.Threading.Tasks.Task DeleteAsync(string id)
        {
            var filter = new BsonDocument("_id", id);
            await _collection.FindOneAndDeleteAsync(filter);
        }

        public void Update(T item)
        {
            var filter = new BsonDocument("_id", new BsonObjectId(new ObjectId(item.Id)));
            _collection.ReplaceOne(filter, item);
        }

        public async System.Threading.Tasks.Task UpdateAsync(T item)
        {
            var filter = new BsonDocument("_id", new BsonObjectId(new ObjectId(item.Id)));
            await _collection.ReplaceOneAsync(filter, item);
        }
    }
}
