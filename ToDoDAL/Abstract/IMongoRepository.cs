using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDoDAL.Abstract
{
    public interface IMongoRepository<T> where T:class
    {
        IEnumerable<T> GetList();
        Task<IEnumerable<T>> GetListAsync();
        T GetItem(string id);
        Task<T> GetItemAsync(string id);
        T Create(T item);
        Task<T> CreateAsync(T item);
        void Delete(string id);
        Task DeleteAsync(string id);
        void Update(T item);
        Task UpdateAsync(T item);
    }
}
