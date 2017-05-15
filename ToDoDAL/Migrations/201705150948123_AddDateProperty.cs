namespace ToDoDAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDateProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ToDoList", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ToDoList", "Date");
        }
    }
}
