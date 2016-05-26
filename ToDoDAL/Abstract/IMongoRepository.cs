using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDoDAL.Abstract
{
    public interface IMongoRepository<T> where T:class
    {
        IEnumerable<T> GetList();
        Task<IEnumerable<T>> GetListAsync();
        T GetItem(int id);
        Task<T> GetItemAsync(int id);
        void Create(T item);
        Task CreateAsync(T item);
        void Delete(int id);
        Task DeleteAsync(int id);
        void Update(T item);
        Task UpdateAsync(T item);
    }
}
