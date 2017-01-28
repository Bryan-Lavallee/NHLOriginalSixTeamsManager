namespace Assignmnet9.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Players", "Team_TeamID", "dbo.Teams");
            DropIndex("dbo.Players", new[] { "Team_TeamID" });
            CreateTable(
                "dbo.PlayerTeams",
                c => new
                    {
                        Player_PlayerId = c.Int(nullable: false),
                        Team_TeamID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Player_PlayerId, t.Team_TeamID })
                .ForeignKey("dbo.Players", t => t.Player_PlayerId, cascadeDelete: true)
                .ForeignKey("dbo.Teams", t => t.Team_TeamID, cascadeDelete: true)
                .Index(t => t.Player_PlayerId)
                .Index(t => t.Team_TeamID);
            
            DropColumn("dbo.Players", "Team_TeamID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Players", "Team_TeamID", c => c.Int());
            DropForeignKey("dbo.PlayerTeams", "Team_TeamID", "dbo.Teams");
            DropForeignKey("dbo.PlayerTeams", "Player_PlayerId", "dbo.Players");
            DropIndex("dbo.PlayerTeams", new[] { "Team_TeamID" });
            DropIndex("dbo.PlayerTeams", new[] { "Player_PlayerId" });
            DropTable("dbo.PlayerTeams");
            CreateIndex("dbo.Players", "Team_TeamID");
            AddForeignKey("dbo.Players", "Team_TeamID", "dbo.Teams", "TeamID");
        }
    }
}
