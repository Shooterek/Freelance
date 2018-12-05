namespace Freelance.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeFieldsLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AnnouncementOffers", "Message", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.Announcements", "Title", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Announcements", "Localization", c => c.String(maxLength: 32));
            AlterColumn("dbo.Photos", "ContentType", c => c.String(maxLength: 32));
            AlterColumn("dbo.Opinions", "Review", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.JobOffers", "Message", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.Jobs", "Title", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Jobs", "Localization", c => c.String(maxLength: 32));
            AlterColumn("dbo.ServiceTypes", "Name", c => c.String(nullable: false, maxLength: 32));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ServiceTypes", "Name", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Jobs", "Localization", c => c.String(maxLength: 31));
            AlterColumn("dbo.Jobs", "Title", c => c.String(nullable: false, maxLength: 25));
            AlterColumn("dbo.JobOffers", "Message", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Opinions", "Review", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Photos", "ContentType", c => c.String(maxLength: 31));
            AlterColumn("dbo.Announcements", "Localization", c => c.String(maxLength: 31));
            AlterColumn("dbo.Announcements", "Title", c => c.String(nullable: false, maxLength: 25));
            AlterColumn("dbo.AnnouncementOffers", "Message", c => c.String(nullable: false, maxLength: 255));
        }
    }
}
