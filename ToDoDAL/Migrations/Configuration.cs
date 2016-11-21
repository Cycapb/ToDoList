namespace ToDoDAL.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<ToDoDAL.Model.TodoContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "ToDoDAL.Model.TodoContext";
        }

        protected override void Seed(ToDoDAL.Model.TodoContext context)
        {

        }
    }
}
