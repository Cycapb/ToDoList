using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ToDoDAL.Model;
using ToDoDomainModels.Repositories;
using Task = System.Threading.Tasks.Task;

namespace ToDoDAL.Concrete
{
    public class EntityRepository<T> : IRepository<T> where T : class
    {
        private readonly TodoContext _context;
        private readonly DbSet<T> _dbSet;

        private bool _disposed = false;

        public EntityRepository()
        {
            _context = new TodoContext();
            _dbSet = _context.Set<T>();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this._disposed = true;
        }

        public virtual Task<IQueryable<T>> GetListAsync()
        {
            return Task.Run(() =>
            {
                IQueryable<T> list = _dbSet;
                return list;
            });
        }

        public virtual async Task<T> GetItemAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual Task CreateAsync(T item)
        {
            return Task.Run(() => _dbSet.Add(item));
        }

        public virtual Task DeleteAsync(int id)
        {
            return Task.Run(async () =>
            {
                var itemToDel = await _dbSet.FindAsync(id);
                if (itemToDel != null)
                {
                    _dbSet.Remove(itemToDel);
                }
            });
        }

        public virtual Task UpdateAsync(T item)
        {
            return Task.Run(() =>
            {
                _context.Entry(item).State = EntityState.Modified;
            });
        }

        public virtual async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
