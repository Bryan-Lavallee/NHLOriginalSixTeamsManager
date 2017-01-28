namespace Assignmnet9.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateCoach : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Coaches", "CoachProfile", c => c.String());
            AddColumn("dbo.Coaches", "PhotoContentType", c => c.String(maxLength: 1000));
            AddColumn("dbo.Coaches", "Photo", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Coaches", "Photo");
            DropColumn("dbo.Coaches", "PhotoContentType");
            DropColumn("dbo.Coaches", "CoachProfile");
        }
    }
}
