using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoDomainModels.Model;
using ToDoDomainModels.Repositories;
using ToDoProviders;

namespace ToDoBussinessLogic.Providers
{
    public class TodoItemProvider : IEntityValueProvider<TodoItem>
    {
        private readonly IRepository<TodoItem> _repository;

        public TodoItemProvider(IRepository<TodoItem> repository)
        {
            _repository = repository;
        }

        public async Task<IQueryable<TodoItem>> GetValuesAsync()
        {
            return (await _repository.GetListAsync());
        }

        public async Task<TodoItem> GetValueAsync(int id)
        {
            var item = await _repository.GetItemAsync(id);
            return item;
        }

        public async Task CreateValueAsync(TodoItem item)
        {
            await _repository.CreateAsync(item);
            await _repository.SaveAsync();
        }

        public async Task UpdateValueAsync(TodoItem item)
        {
            await _repository.UpdateAsync(item);
            await _repository.SaveAsync();
        }

        public async Task UpdateValuesAsync(IEnumerable<TodoItem> items)
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