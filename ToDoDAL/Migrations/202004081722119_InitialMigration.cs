namespace ToDoDAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TodoGroup",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        UserId = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TodoItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 1024),
                        GroupId = c.Int(nullable: false),
                        IsFinished = c.Boolean(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TodoGroup", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.GroupId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TodoItem", "GroupId", "dbo.TodoGroup");
            DropIndex("dbo.TodoItem", new[] { "GroupId" });
            DropTable("dbo.TodoItem");
            DropTable("dbo.TodoGroup");
        }
    }
}
