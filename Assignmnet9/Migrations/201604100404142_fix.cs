namespace Assignmnet9.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Teams", "URLTeam", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Teams", "URLTeam");
        }
    }
}
