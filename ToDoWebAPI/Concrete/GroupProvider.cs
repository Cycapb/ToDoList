using System.Collections.Generic;
using System.Linq;
using ToDoDAL.Abstract;
using ToDoDAL.Model;
using ToDoWebAPI.Abstract;

namespace ToDoWebAPI.Concrete
{
    public class GroupProvider:IEntityValueProvider<Group>
    {
        private readonly IRepository<Group> _repository;

        public GroupProvider(IRepository<Group> repository)
        {
            _repository = repository;
        }

        public IEnumerable<Group> GetValues()
        {
            return _repository.GetList()
                .Select(x=> new Group()
                {
                    GroupId = x.GroupId,
                    Name = x.Name,
                    UserId = x.UserId
                });

        }

        public Group GetValue(int id)
        {
            var item = _repository.GetItem(id);
            return new Group()
            {
                GroupId = item.GroupId,
                Name = item.Name,
                UserId = item.UserId
            };
        }

        public void CreateValue(Group item)
        {
            _repository.Create(item);
            _repository.Save();
        }

        public void UpdateValue(Group item)
        {
            _repository.Update(item);
            _repository.Save();
        }

        public void DeleteValue(int id)
        {
            _repository.Delete(id);
            _repository.Save();
        }
    }
}