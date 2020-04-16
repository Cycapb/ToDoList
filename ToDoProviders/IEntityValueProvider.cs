using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoProviders
{
    public interface IEntityValueProvider<T> where T:class
    {
        Task<IQueryable<T>> GetValuesAsync();

        Task<T> GetValueAsync(int id);

        Task CreateValueAsync(T item);

        Task UpdateValueAsync(T item);

        Task UpdateValuesAsync(IEnumerable<T> items);

        Task DeleteValueAsync(int id);
    }
}
