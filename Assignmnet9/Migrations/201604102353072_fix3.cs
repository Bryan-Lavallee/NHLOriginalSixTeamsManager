namespace Assignmnet9.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix3 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Players", "Team");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Players", "Team", c => c.String(nullable: false));
        }
    }
}
