namespace Freelance.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedLocationColumnName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Announcements", "Localization", c => c.String());
            DropColumn("dbo.Announcements", "Location");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Announcements", "Location", c => c.String());
            DropColumn("dbo.Announcements", "Localization");
        }
    }
}
