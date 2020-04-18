using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using ToDoDAL.Abstract;

namespace ToDoDAL.Core.Model
{
    public class EntityRepositoryCore<TEntity, TContext> : IRepository<TEntity>
        where TEntity : class
        where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly DbSet<TEntity> _dbSet;

        private bool _disposed = false;

        public EntityRepositoryCore(TContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
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

        public virtual Task<IQueryable<TEntity>> GetListAsync()
        {
            return Task.Run(() =>
            {
                IQueryable<TEntity> list = _dbSet;
                return list;
            });
        }

        public virtual async Task<TEntity> GetItemAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual Task CreateAsync(TEntity item)
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

        public virtual Task UpdateAsync(TEntity item)
        {
            return Task.Run(() => _context.Entry(item).State = EntityState.Modified);
        }

        public virtual async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
