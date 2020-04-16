using System.Collections.Generic;
using System.Linq;
using ToDoDAL.Abstract;
using ToDoDAL.Model.MongoModel;
using ToDoProviders;

namespace ToDoBussinessLogic.Providers
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

        public TaskGroup GetValue(string id)
        {
            return _repository.GetItem(id);
        }

        public TaskGroup CreateValue(TaskGroup item)
        {
            return _repository.Create(item);
        }

        public void UpdateValue(TaskGroup item)
        {
            _repository.Update(item);
        }

        public void DeleteValue(string id)
        {
            _repository.Delete(id);
        }
    }
}