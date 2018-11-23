namespace Freelance.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNavigationPropertiesForAnOpinion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Opinions", "AnnouncementOfferId", c => c.Int());
            AddColumn("dbo.Opinions", "JobOfferId", c => c.Int());
            CreateIndex("dbo.Opinions", "AnnouncementOfferId");
            CreateIndex("dbo.Opinions", "JobOfferId");
            AddForeignKey("dbo.Opinions", "AnnouncementOfferId", "dbo.AnnouncementOffers", "AnnouncementOfferId");
            AddForeignKey("dbo.Opinions", "JobOfferId", "dbo.JobOffers", "JobOfferId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Opinions", "JobOfferId", "dbo.JobOffers");
            DropForeignKey("dbo.Opinions", "AnnouncementOfferId", "dbo.AnnouncementOffers");
            DropIndex("dbo.Opinions", new[] { "JobOfferId" });
            DropIndex("dbo.Opinions", new[] { "AnnouncementOfferId" });
            DropColumn("dbo.Opinions", "JobOfferId");
            DropColumn("dbo.Opinions", "AnnouncementOfferId");
        }
    }
}
