using System;
using System.Collections.Generic;
using System.Linq;
using ToDoDAL.Abstract;
using ToDoWebAPI.Abstract;
using ToDoDAL.Model.MongoModel;

namespace ToDoWebAPI.Concrete
{
    public class MongoTaskValueProvider:IMongoValueProvider<Task>
    {
        private readonly IMongoRepository<Task> _repository;

        public MongoTaskValueProvider(IMongoRepository<Task> repostitory)
        {
            _repository = repostitory;
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