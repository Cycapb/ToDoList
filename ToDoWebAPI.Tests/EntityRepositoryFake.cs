using System.Collections.Generic;
using ToDoDAL.Concrete;
using ToDoDAL.Model;
using System.Data.Entity;

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

        public override IEnumerable<T> GetList()
        {
            return _dbSet;
        }

        public override T GetItem(int id)
        {
            return _dbSet.Find(id);
        }

        public override void Create(T item)
        {
            _dbSet.Add(item);
        }

        public override void Delete(int id)
        {
            T item = this.GetItem(id);
            if (item != null)
            {
                _dbSet.Remove(item);
            }
        }

        public override void Update(T item)
        {
            UpdateItem(item);
        }

        public override void Save()
        {
            _context.SaveChanges();
        }

        public virtual void UpdateItem(T item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }
    }
}
