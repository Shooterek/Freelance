namespace Freelance.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameSummaryProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Announcements", "Title", c => c.String());
            DropColumn("dbo.Announcements", "Summary");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Announcements", "Summary", c => c.String());
            DropColumn("dbo.Announcements", "Title");
        }
    }
}
