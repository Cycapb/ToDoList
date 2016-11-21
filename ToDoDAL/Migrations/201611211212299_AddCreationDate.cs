namespace ToDoDAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCreationDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ToDoList", "CreationDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ToDoList", "CreationDate");
        }
    }
}
