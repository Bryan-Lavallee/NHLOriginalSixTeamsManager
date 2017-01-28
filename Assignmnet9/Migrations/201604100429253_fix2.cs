namespace Assignmnet9.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Players", "Profile", c => c.String());
            AddColumn("dbo.Players", "PlayerURL", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Players", "PlayerURL");
            DropColumn("dbo.Players", "Profile");
        }
    }
}
