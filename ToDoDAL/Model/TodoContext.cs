using System.Data.Entity;

namespace ToDoDAL.Model
{
    public class TodoContext:DbContext
    {
        public TodoContext():base("name=todoEntities")
        {
            
        }

        public DbSet<ToDoList> ToDoList { get; set; }
        public DbSet<Group> Group { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>()
                .HasKey(s => s.GroupId);
            modelBuilder.Entity<Group>()
                .Property(s => s.Name)
                .HasMaxLength(50);
            modelBuilder.Entity<Group>()
                .Property(s => s.UserId)
                .IsRequired();
        }
    }
}