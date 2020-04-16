using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoDAL.Abstract;
using ToDoDAL.Model;
using ToDoProviders;

namespace ToDoBussinessLogic.Providers
{
    public class GroupProvider : IEntityValueProvider<TodoGroup>
    {
        private readonly IRepository<TodoGroup> _repository;

        public GroupProvider(IRepository<TodoGroup> repository)
        {
            _repository = repository;
        }

        public async Task<IQueryable<TodoGroup>> GetValuesAsync()
        {
            return await _repository.GetListAsync();
        }

        public async Task<TodoGroup> GetValueAsync(int id)
        {
            var item = await _repository.GetItemAsync(id);
            return item;
        }

        public async Task CreateValueAsync(TodoGroup item)
        {
            await _repository.CreateAsync(item);
            await _repository.SaveAsync();
        }

        public async Task UpdateValueAsync(TodoGroup item)
        {
            await _repository.UpdateAsync(item);
            await _repository.SaveAsync();
        }

        public async Task UpdateValuesAsync(IEnumerable<TodoGroup> items)
        {
            foreach (var group in items)
            {
                await _repository.UpdateAsync(group);
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