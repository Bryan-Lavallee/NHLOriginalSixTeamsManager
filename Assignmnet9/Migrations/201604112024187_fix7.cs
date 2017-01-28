namespace Assignmnet9.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Players", "PhotoContentType", c => c.String(maxLength: 1000));
            AddColumn("dbo.Players", "Photo", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Players", "Photo");
            DropColumn("dbo.Players", "PhotoContentType");
        }
    }
}
