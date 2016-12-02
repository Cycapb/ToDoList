using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoDAL.Abstract;
using ToDoDAL.Model;
using ToDoWebAPI.Abstract;
using WebGrease.Css.Extensions;

namespace ToDoWebAPI.Concrete
{
    public class ToDoListProvider:IEntityValueProvider<ToDoList>
    {
        private readonly IRepository<ToDoList> _repository;

        public ToDoListProvider(IRepository<ToDoList> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ToDoList>> GetValuesAsync()
        {
            return (await _repository.GetListAsync())
                .Select(x => new ToDoList()
                {
                    Comment = x.Comment,
                    Name = x.Name,
                    GroupId = x.GroupId,
                    GroupName = x.Group.Name,
                    StatusId = x.StatusId,
                    UserId = x.UserId,
                    NoteId = x.NoteId
                }); 
        }

        public async Task<ToDoList> GetValueAsync(int id)
        {
            var item = await _repository.GetItemAsync(id);
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
                    GroupName = item.Group.Name,
                    StatusId = item.StatusId,
                    UserId = item.UserId,
                    NoteId = item.NoteId
                };
            }
        }

        public async Task CreateValueAsync(ToDoList item)
        {
            await _repository.CreateAsync(item);
            await _repository.SaveAsync();
        }

        public async Task UpdateValueAsync(ToDoList item)
        {
            await _repository.UpdateAsync(item);
            await _repository.SaveAsync();
        }

        public async Task UpdateValuesAsync(IEnumerable<ToDoList> items)
        {
            foreach (var item in items)
            {
                await _repository.UpdateAsync(item);
            }
            await _repository.SaveAsync();
        }

        public async Task DeleteValueAsync(int id)
        {
                await _repository.DeleteAsync(id);
                await _repository.SaveAsync();
        }
    }
}