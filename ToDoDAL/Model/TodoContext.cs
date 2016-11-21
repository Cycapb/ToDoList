namespace ToDoDAL.Model
{
    using System.Data.Entity;

    public partial class TodoContext : DbContext
    {
        public TodoContext()
            : base("name=TodoEntities")
        {
            
        }

        public virtual DbSet<Group> Group { get; set; }
        public virtual DbSet<ToDoList> ToDoList { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Group>()
                .Property(e => e.UserId)
                .IsUnicode(false);

            modelBuilder.Entity<Group>()
                .HasMany(e => e.ToDoList)
                .WithRequired(e => e.Group)
                .HasForeignKey(e => e.GroupId)
                .WillCascadeOnDelete(false);
            
            modelBuilder.Entity<ToDoList>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<ToDoList>()
                .Property(e => e.Comment)
                .IsUnicode(false);

            modelBuilder.Entity<ToDoList>()
                .Property(e => e.UserId)
                .IsUnicode(false);
        }
    }
}
