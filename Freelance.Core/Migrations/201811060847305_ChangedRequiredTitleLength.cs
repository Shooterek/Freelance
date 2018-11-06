namespace Freelance.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedRequiredTitleLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Announcements", "Title", c => c.String(nullable: false, maxLength: 25));
            AlterColumn("dbo.Jobs", "Title", c => c.String(nullable: false, maxLength: 25));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Jobs", "Title", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Announcements", "Title", c => c.String(nullable: false, maxLength: 255));
        }
    }
}
