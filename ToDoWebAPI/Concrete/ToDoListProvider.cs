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

        public async Task<IQueryable<ToDoList>> GetValuesAsync()
        {
            return (await _repository.GetListAsync());
        }

        public async Task<ToDoList> GetValueAsync(int id)
        {
            var item = await _repository.GetItemAsync(id);
            return item;
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