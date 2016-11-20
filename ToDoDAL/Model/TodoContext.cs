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
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Group>()
                .Property(s => s.UserId)
                .IsRequired();

            modelBuilder.Entity<Group>()
                .HasMany<ToDoList>(s => s.ToDoList)
                .WithRequired(s => s.Group);

            modelBuilder.Entity<ToDoList>()
                .HasKey(s => s.NoteId);

            modelBuilder.Entity<ToDoList>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<ToDoList>()
                .Property(p => p.Comment)
                .IsOptional();

            modelBuilder.Entity<ToDoList>()
                .Property(p => p.StatusId)
                .IsRequired();
            modelBuilder.Entity<ToDoList>()
                .Property(p => p.UserId)
                .IsRequired();
        }
    }
}