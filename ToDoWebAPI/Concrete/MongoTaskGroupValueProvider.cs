using System;
using System.Collections.Generic;
using System.Linq;
using ToDoWebAPI.Abstract;
using ToDoDAL.Concrete;
using ToDoDAL.Model.MongoModel;

namespace ToDoWebAPI.Concrete
{
    public class MongoTaskGroupValueProvider : IMongoValueProvider<TaskGroup>
    {
        private readonly MongoRepository<TaskGroup> _repository;

        public MongoTaskGroupValueProvider()
        {
            _repository = new MongoRepository<TaskGroup>();
        }

        public IEnumerable<TaskGroup> GetValues()
        {
            return _repository.GetList().ToList();
        }

        public TaskGroup GetValue(int id)
        {
            return _repository.GetItem(id);
        }

        public void CreateValue(TaskGroup item)
        {
            throw new NotImplementedException();
        }

        public void UpdateValue(TaskGroup item)
        {
            throw new NotImplementedException();
        }

        public void DeleteValue(int id)
        {
            throw new NotImplementedException();
        }
    }
}