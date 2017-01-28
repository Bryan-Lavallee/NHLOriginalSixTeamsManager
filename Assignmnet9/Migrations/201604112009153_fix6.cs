namespace Assignmnet9.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Players", "Profile", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Players", "Profile");
        }
    }
}
