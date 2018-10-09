namespace Freelance.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOffers : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Offers", newName: "JobOffers");
            DropPrimaryKey("dbo.JobOffers");
            DropColumn("dbo.JobOffers", "OfferId");
            CreateTable(
                "dbo.AnnouncementOffers",
                c => new
                    {
                        AnnouncementOfferId = c.Int(nullable: false, identity: true),
                        AnnouncementId = c.Int(nullable: false),
                        SubmissionDate = c.DateTime(nullable: false),
                        OffererId = c.String(maxLength: 128),
                        ProposedRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.AnnouncementOfferId)
                .ForeignKey("dbo.Announcements", t => t.AnnouncementId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.OffererId)
                .Index(t => t.AnnouncementId)
                .Index(t => t.OffererId);
            
            AddColumn("dbo.JobOffers", "JobOfferId", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.JobOffers", "SubmissionDate", c => c.DateTime(nullable: false));
            AddPrimaryKey("dbo.JobOffers", "JobOfferId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.JobOffers", "OfferId", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.AnnouncementOffers", "OffererId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AnnouncementOffers", "AnnouncementId", "dbo.Announcements");
            DropIndex("dbo.AnnouncementOffers", new[] { "OffererId" });
            DropIndex("dbo.AnnouncementOffers", new[] { "AnnouncementId" });
            DropPrimaryKey("dbo.JobOffers");
            DropColumn("dbo.JobOffers", "SubmissionDate");
            DropColumn("dbo.JobOffers", "JobOfferId");
            DropTable("dbo.AnnouncementOffers");
            AddPrimaryKey("dbo.JobOffers", "OfferId");
            RenameTable(name: "dbo.JobOffers", newName: "Offers");
        }
    }
}
