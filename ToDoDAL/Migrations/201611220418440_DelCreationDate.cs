namespace ToDoDAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DelCreationDate : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ToDoList", "CreationDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ToDoList", "CreationDate", c => c.DateTime(nullable: false));
        }
    }
}
