using Microsoft.EntityFrameworkCore;
using ToDoDomainModels.Model;

namespace ToDoDAL.Core.Model
{
    public class TodoContextCore : DbContext
    {
        public TodoContextCore(DbContextOptions<TodoContextCore> dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<TodoGroup> TodoGroups { get; set; }

        public DbSet<TodoItem> TodoItems { get; set; }
    }
}
