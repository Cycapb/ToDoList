using System;
using System.Collections.Generic;
using System.Linq;
using ToDoDAL.Abstract;
using ToDoWebAPI.Abstract;
using ToDoDAL.Model.MongoModel;

namespace ToDoWebAPI.Concrete
{
    public class MongoTaskGroupValueProvider : IMongoValueProvider<TaskGroup>
    {
        private readonly IMongoRepository<TaskGroup> _repository;

        public MongoTaskGroupValueProvider(IMongoRepository<TaskGroup> repository)
        {
            _repository = repository;
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