using System.Collections.Generic;
using System.Linq;
using ToDoDAL.Abstract;
using ToDoDAL.Model.MongoModel;
using ToDoProviders;

namespace ToDoBussinessLogic.Providers
{
    public class MongoTaskValueProvider : IMongoValueProvider<Task>
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

        public Task GetValue(string id)
        {
            return _repository.GetItem(id);
        }

        public Task CreateValue(Task item)
        {
            return _repository.Create(item);
        }

        public void UpdateValue(Task item)
        {
            _repository.Update(item);
        }

        public void DeleteValue(string id)
        {
            _repository.Delete(id);
        }
    }
}