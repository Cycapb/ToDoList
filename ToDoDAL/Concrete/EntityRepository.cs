using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using ToDoDAL.Abstract;
using ToDoDAL.Model;

namespace ToDoDAL.Concrete
{
    public class EntityRepository<T>:IRepository<T> where T:class
    {
        private readonly todoEntities _context;
        private readonly DbSet<T> _dbSet;

        private bool _disposed = false;
        
        public EntityRepository()
        {
            _context = new todoEntities();
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

        public IEnumerable<T> GetList()
        {
            return _dbSet;
        }

        public Task<IEnumerable<T>> GetListAsync()
        {
            return Task.Run(() =>
            {
                IEnumerable<T> list = _dbSet;
                return list;
            });
        }

        public T GetItem(int id)
        {
            return _dbSet.Find(id);
        }

        public async Task<T> GetItemAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Create(T item)
        {
            _dbSet.Add(item);
        }

        public Task CreateAsync(T item)
        {
            return Task.Run(()=>_dbSet.Add(item));
        }

        public void Delete(int id)
        {
            var itemToDel = _dbSet.Find(id);
            if (itemToDel != null)
            {
                _dbSet.Remove(itemToDel);
            }
        }

        public Task DeleteAsync(int id)
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

        public void Update(T item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        public Task UpdateAsync(T item)
        {
            return Task.Run(() =>
            {
                _context.Entry(item).State = EntityState.Modified;
            });
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
