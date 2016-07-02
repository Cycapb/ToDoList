using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDoDAL.Abstract
{
    public interface IRepository<T>:IDisposable where T:class
    {
        Task<IEnumerable<T>> GetListAsync();
        Task<T> GetItemAsync(int id);
        Task CreateAsync(T item);
        Task DeleteAsync(int id);
        Task UpdateAsync(T item);
        Task SaveAsync();
    }
}
