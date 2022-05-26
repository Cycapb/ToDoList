using System.Collections.Generic;
using System.Linq;
using ToDoDomainModels.Repositories;
using ToDoProviders;

namespace ToDoBussinessLogic.Providers
{
    public class MongoTaskValueProvider : IMongoValueProvider<ToDoDomainModels.Model.Mongo.Task>
    {
        private readonly IMongoRepository<ToDoDomainModels.Model.Mongo.Task> _repository;

        public MongoTaskValueProvider(IMongoRepository<ToDoDomainModels.Model.Mongo.Task> repostitory)
        {
            _repository = repostitory;
        }

        public IEnumerable<ToDoDomainModels.Model.Mongo.Task> GetValues()
        {
            return _repository.GetList().ToList();
        }

        public ToDoDomainModels.Model.Mongo.Task GetValue(string id)
        {
            return _repository.GetItem(id);
        }

        public ToDoDomainModels.Model.Mongo.Task CreateValue(ToDoDomainModels.Model.Mongo.Task item)
        {
            return _repository.Create(item);
        }

        public void UpdateValue(ToDoDomainModels.Model.Mongo.Task item)
        {
            _repository.Update(item);
        }

        public void DeleteValue(string id)
        {
            _repository.Delete(id);
        }
    }
}