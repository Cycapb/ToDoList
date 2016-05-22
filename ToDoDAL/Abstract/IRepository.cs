using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDoDAL.Abstract
{
    public interface IRepository<T>:IDisposable where T:class
    {
        IEnumerable<T> GetList();
        Task<IEnumerable<T>> GetListAsync();
        void Create(T item);
        Task CreateAsync(T item);
        void Delete(int id);
        Task DeleteAsync(int id);
        void Update(T item);
        Task UpdateAsync(T item);
        void Save();
        Task SaveAsync();
    }
}
