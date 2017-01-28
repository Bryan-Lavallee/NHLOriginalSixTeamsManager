namespace Assignmnet9.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix31 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.MediaItems", new[] { "Team_TeamID" });
            AlterColumn("dbo.MediaItems", "Team_TeamID", c => c.Int());
            CreateIndex("dbo.MediaItems", "Team_TeamID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.MediaItems", new[] { "Team_TeamID" });
            AlterColumn("dbo.MediaItems", "Team_TeamID", c => c.Int(nullable: false));
            CreateIndex("dbo.MediaItems", "Team_TeamID");
        }
    }
}
