using System;
using System.Collections.Generic;
using System.Linq;
using ToDoDAL.Concrete;
using ToDoWebAPI.Abstract;
using ToDoDAL.Model.MongoModel;

namespace ToDoWebAPI.Concrete
{
    public class MongoTaskValueProvider:IMongoValueProvider<Task>
    {
        private readonly MongoRepository<Task> _repository;

        public MongoTaskValueProvider()
        {
            _repository = new MongoRepository<Task>();
        }

        public IEnumerable<Task> GetValues()
        {
            return _repository.GetList().ToList();
        }

        public Task GetValue(int id)
        {
            return _repository.GetItem(id);
        }

        public void CreateValue(Task item)
        {
            throw new NotImplementedException();
        }

        public void UpdateValue(Task item)
        {
            throw new NotImplementedException();
        }

        public void DeleteValue(int id)
        {
            _repository.Delete(id);
        }
    }
}