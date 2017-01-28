namespace Assignmnet9.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix32 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MediaItems", "Player_PlayerId", "dbo.Players");
            DropIndex("dbo.MediaItems", new[] { "Player_PlayerId" });
            DropColumn("dbo.MediaItems", "Player_PlayerId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MediaItems", "Player_PlayerId", c => c.Int());
            CreateIndex("dbo.MediaItems", "Player_PlayerId");
            AddForeignKey("dbo.MediaItems", "Player_PlayerId", "dbo.Players", "PlayerId");
        }
    }
}
