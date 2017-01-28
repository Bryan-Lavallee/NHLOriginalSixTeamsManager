namespace Assignmnet9.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCoach : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Coaches",
                c => new
                    {
                        CoachId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        TeamName = c.String(),
                        WinPercentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NumberOfGamesCoached = c.Int(nullable: false),
                        YearsCoached = c.Int(nullable: false),
                        Team_TeamID = c.Int(),
                    })
                .PrimaryKey(t => t.CoachId)
                .ForeignKey("dbo.Teams", t => t.Team_TeamID)
                .Index(t => t.Team_TeamID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Coaches", "Team_TeamID", "dbo.Teams");
            DropIndex("dbo.Coaches", new[] { "Team_TeamID" });
            DropTable("dbo.Coaches");
        }
    }
}
