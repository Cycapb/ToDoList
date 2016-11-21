namespace ToDoDAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ToDoLists", "GroupId", "dbo.Groups");
            AlterColumn("dbo.Group", "Name", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.Group", "UserId", c => c.String(nullable: false, unicode: false));
            AlterColumn("dbo.ToDoList", "Name", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.ToDoList", "Comment", c => c.String(maxLength: 250, unicode: false));
            AlterColumn("dbo.ToDoList", "UserId", c => c.String(nullable: false, unicode: false));
            AddForeignKey("dbo.ToDoList", "GroupId", "dbo.Group", "GroupId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ToDoList", "GroupId", "dbo.Group");
            AlterColumn("dbo.ToDoList", "UserId", c => c.String(nullable: false));
            AlterColumn("dbo.ToDoList", "Comment", c => c.String());
            AlterColumn("dbo.ToDoList", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Group", "UserId", c => c.String(nullable: false));
            AlterColumn("dbo.Group", "Name", c => c.String(nullable: false, maxLength: 50));
            AddForeignKey("dbo.ToDoLists", "GroupId", "dbo.Groups", "GroupId", cascadeDelete: true);
        }
    }
}
