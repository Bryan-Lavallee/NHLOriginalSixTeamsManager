namespace Assignmnet9.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix41 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Positions", "Player_PlayerId", c => c.Int());
            CreateIndex("dbo.Positions", "Player_PlayerId");
            AddForeignKey("dbo.Positions", "Player_PlayerId", "dbo.Players", "PlayerId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Positions", "Player_PlayerId", "dbo.Players");
            DropIndex("dbo.Positions", new[] { "Player_PlayerId" });
            DropColumn("dbo.Positions", "Player_PlayerId");
        }
    }
}
