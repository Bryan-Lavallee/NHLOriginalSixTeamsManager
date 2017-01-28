namespace Assignmnet9.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MediaItems", "Player_PlayerId", c => c.Int());
            CreateIndex("dbo.MediaItems", "Player_PlayerId");
            AddForeignKey("dbo.MediaItems", "Player_PlayerId", "dbo.Players", "PlayerId");
            DropColumn("dbo.Teams", "URLTeam");
            DropColumn("dbo.Players", "PlayerURL");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Players", "PlayerURL", c => c.String());
            AddColumn("dbo.Teams", "URLTeam", c => c.String());
            DropForeignKey("dbo.MediaItems", "Player_PlayerId", "dbo.Players");
            DropIndex("dbo.MediaItems", new[] { "Player_PlayerId" });
            DropColumn("dbo.MediaItems", "Player_PlayerId");
        }
    }
}
