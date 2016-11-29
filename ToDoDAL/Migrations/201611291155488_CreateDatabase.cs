namespace ToDoDAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Group",
                c => new
                    {
                        GroupId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50, unicode: false),
                        UserId = c.String(nullable: false, unicode: false),
                    })
                .PrimaryKey(t => t.GroupId);
            
            CreateTable(
                "dbo.ToDoList",
                c => new
                    {
                        NoteId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50, unicode: false),
                        Comment = c.String(maxLength: 250, unicode: false),
                        GroupId = c.Int(nullable: false),
                        StatusId = c.Boolean(nullable: false),
                        UserId = c.String(nullable: false, unicode: false),
                    })
                .PrimaryKey(t => t.NoteId)
                .ForeignKey("dbo.Group", t => t.GroupId)
                .Index(t => t.GroupId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ToDoList", "GroupId", "dbo.Group");
            DropIndex("dbo.ToDoList", new[] { "GroupId" });
            DropTable("dbo.ToDoList");
            DropTable("dbo.Group");
        }
    }
}
