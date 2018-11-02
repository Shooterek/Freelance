namespace Freelance.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDataAnnotations : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AnnouncementOffers", "OffererId", "dbo.AspNetUsers");
            DropForeignKey("dbo.JobOffers", "OffererId", "dbo.AspNetUsers");
            DropIndex("dbo.AnnouncementOffers", new[] { "OffererId" });
            DropIndex("dbo.JobOffers", new[] { "OffererId" });
            AlterColumn("dbo.AnnouncementOffers", "OffererId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Announcements", "Title", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Announcements", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.JobOffers", "OffererId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Jobs", "Title", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Jobs", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.ServiceTypes", "Name", c => c.String(nullable: false, maxLength: 30));
            CreateIndex("dbo.AnnouncementOffers", "OffererId");
            CreateIndex("dbo.JobOffers", "OffererId");
            AddForeignKey("dbo.AnnouncementOffers", "OffererId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.JobOffers", "OffererId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobOffers", "OffererId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AnnouncementOffers", "OffererId", "dbo.AspNetUsers");
            DropIndex("dbo.JobOffers", new[] { "OffererId" });
            DropIndex("dbo.AnnouncementOffers", new[] { "OffererId" });
            AlterColumn("dbo.ServiceTypes", "Name", c => c.String());
            AlterColumn("dbo.Jobs", "Description", c => c.String());
            AlterColumn("dbo.Jobs", "Title", c => c.String());
            AlterColumn("dbo.JobOffers", "OffererId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Announcements", "Description", c => c.String());
            AlterColumn("dbo.Announcements", "Title", c => c.String());
            AlterColumn("dbo.AnnouncementOffers", "OffererId", c => c.String(maxLength: 128));
            CreateIndex("dbo.JobOffers", "OffererId");
            CreateIndex("dbo.AnnouncementOffers", "OffererId");
            AddForeignKey("dbo.JobOffers", "OffererId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AnnouncementOffers", "OffererId", "dbo.AspNetUsers", "Id");
        }
    }
}
