namespace Assignmnet9.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix_player : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Players", "Profile");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Players", "Profile", c => c.String());
        }
    }
}
