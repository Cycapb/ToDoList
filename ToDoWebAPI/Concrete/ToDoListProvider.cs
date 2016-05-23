using System.Collections.Generic;
using System.Linq;
using ToDoDAL.Abstract;
using ToDoDAL.Model;
using ToDoWebAPI.Abstract;

namespace ToDoWebAPI.Concrete
{
    public class ToDoListProvider:IEntityValueProvider<ToDoList>
    {
        private readonly IRepository<ToDoList> _repository;

        public ToDoListProvider(IRepository<ToDoList> repository)
        {
            _repository = repository;
        }

        public IEnumerable<ToDoList> GetValues()
        {
            var list = _repository.GetList()
                .Select(x => new ToDoList()
                {
                    Comment = x.Comment,
                    Name = x.Name,
                    GroupId = x.GroupId,
                    StatusId = x.StatusId,
                    UserId = x.UserId,
                    NoteId = x.NoteId
                });
            return list;
        }

        public ToDoList GetValue(int id)
        {
            var item = _repository.GetItem(id);
            if (item == null)
            {
                return null;
            }
            else
            {
                return new ToDoList()
                {
                    Comment = item.Comment,
                    Name = item.Name,
                    GroupId = item.GroupId,
                    StatusId = item.StatusId,
                    UserId = item.UserId,
                    NoteId = item.NoteId
                };
            }
        }
        
        public void CreateValue(ToDoList item)
        {
            _repository.Create(item);
            _repository.Save();
        }
        
        public void UpdateValue(ToDoList item)
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