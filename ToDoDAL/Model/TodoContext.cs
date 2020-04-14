namespace ToDoDAL.Model
{
    using System.Data.Entity;

    public partial class TodoContext : DbContext
    {
        public TodoContext()
            : base("name=TodoEntities")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<TodoContext, Migrations.Configuration>("TodoEntities"));
        }

        public virtual DbSet<TodoGroup> Groups { get; set; }

        public virtual DbSet<TodoItem> TodoItems { get; set; }
    }
}
