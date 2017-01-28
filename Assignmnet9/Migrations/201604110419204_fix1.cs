namespace Assignmnet9.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Players", "TeamName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Players", "TeamName", c => c.String(nullable: false));
        }
    }
}
