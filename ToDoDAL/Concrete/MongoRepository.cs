using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using ToDoDAL.Abstract;

namespace ToDoDAL.Concrete
{
    public class MongoRepository<T> where T:IEntity<T>
    {
        private readonly MongoClient _mongoClient;
        private readonly IMongoDatabase _mongoDatabase;
        private readonly IMongoCollection<T> _collection; 
        private bool _disposed = false;

        public MongoRepository()
        {
            _mongoClient = new MongoClient("mongodb://192.168.1.144:27017");
            _mongoDatabase = _mongoClient.GetDatabase("todo");
            _collection = _mongoDatabase.GetCollection<T>(GetCollectionNameFromType(typeof (T)));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    this.Dispose();
                }
            }
            _disposed = true;
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

        public T GetItem(int id)
        {
            var filter = new BsonDocument("_id", id);
            return _collection.AsQueryable().SingleOrDefault(x => x.Id == id);
        }

        public Task<T> GetItemAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Create(T item)
        {
            _collection.InsertOne(item);
        }

        public Task CreateAsync(T item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            var filter = new BsonDocument("_id",id);
            _collection.FindOneAndDelete(filter);
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(T item)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(T item)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync()
        {
            throw new NotImplementedException();
        }
    }
}
