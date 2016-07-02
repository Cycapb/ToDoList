using System.Collections.Generic;
using ToDoDAL.Concrete;
using ToDoDAL.Model;
using System.Data.Entity;
using System.Threading.Tasks;

namespace ToDoWebAPI.Tests
{
    public class EntityRepositoryFake<T>:EntityRepository<T> where T:class 
    {
        private todoEntities _context;
        private DbSet<T> _dbSet;

        public EntityRepositoryFake() { }

        public EntityRepositoryFake(todoEntities context )  
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual void UpdateItem(T item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        public override Task<IEnumerable<T>> GetListAsync()
        {
            return Task.Run(() =>
            {
                IEnumerable<T> list = _dbSet;
                return list;
            });
        }

        public override Task<T> GetItemAsync(int id)
        {
            return Task.Run(() => _dbSet.Find(id));
        }

        public override async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public override Task UpdateAsync(T item)
        {
            UpdateItem(item);
            return Task.Run(() => item);
        }

        public override Task CreateAsync(T item)
        {
            return Task.Run(() => _dbSet.Add(item));
        }

        public override Task DeleteAsync(int id)
        {
            return Task.Run(() =>
            {
                var item = _dbSet.Find(id);
                if (item != null)
                {
                    _dbSet.Remove(item);
                }
            });
        }
    }
}
