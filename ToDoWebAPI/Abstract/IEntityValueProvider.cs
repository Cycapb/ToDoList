using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDoWebAPI.Abstract
{
    public interface IEntityValueProvider<T> where T:class
    {

        Task<IEnumerable<T>> GetValuesAsync();
        Task<T> GetValueAsync(int id);
        Task CreateValueAsync(T item);
        Task UpdateValueAsync(T item);
        Task UpdateValuesAsync(IEnumerable<T> items);
        Task DeleteValueAsync(int id);
    }
}
