using System;
using System.Collections.Generic;
using System.Linq;
using ThreadTask = System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ToDoDAL.Abstract;
using Task = ToDoDAL.Model.MongoModel.Task;

namespace ToDoDAL.Concrete
{
    public class MongoRepository<T>:IMongoRepository<T> where T:class,IEntity
    {
        private readonly MongoClient _mongoClient;
        private readonly IMongoDatabase _mongoDatabase;
        private readonly IMongoCollection<T> _collection; 

        public MongoRepository()
        {
            _mongoClient = new MongoClient("mongodb://192.168.1.144:27017");
            _mongoDatabase = _mongoClient.GetDatabase("todo");
            _collection = _mongoDatabase.GetCollection<T>(GetCollectionNameFromType(typeof (T)));
        }
        
        private string GetCollectionNameFromType(Type entitytype)
        {
            return entitytype.Name.ToLower();
        }

        public IEnumerable<T> GetList()
        {
            return _collection.AsQueryable().ToList();
        }

        public async ThreadTask.Task<IEnumerable<T>> GetListAsync()
        {
            return await _collection.AsQueryable().ToListAsync();
        }

        public T GetItem(int id)
        {
            return _collection.AsQueryable().SingleOrDefault(x => x.Id == id);
        }

        public async ThreadTask.Task<T> GetItemAsync(int id)
        {
            return await _collection.AsQueryable().Where(x => x.Id == id).SingleOrDefaultAsync();
        }

        public void Create(T item)
        {
            _collection.InsertOne(item);
        }

        public async ThreadTask.Task CreateAsync(T item)
        {
            await _collection.InsertOneAsync(item);
        }

        public void Delete(int id)
        {
            var filter = new BsonDocument("_id",id);
            _collection.FindOneAndDelete(filter);
        }

        public async ThreadTask.Task DeleteAsync(int id)
        {
            var filter = new BsonDocument("_id", id);
            await _collection.FindOneAndDeleteAsync(filter);
        }

        public void Update(T item)
        {
            var filter = new BsonDocument("_id",item.Id);
            _collection.ReplaceOne(filter, item);
        }

        public async ThreadTask.Task UpdateAsync(T item)
        {
            var filter = new BsonDocument("_id", item.Id);
            await _collection.ReplaceOneAsync(filter, item);
        }
    }
}
