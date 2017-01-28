namespace Assignmnet9.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class teamName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Players", "TeamName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Players", "TeamName");
        }
    }
}
