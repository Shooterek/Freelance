namespace Freelance.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsAcceptedAndIsFinishedColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AnnouncementOffers", "IsAccepted", c => c.Boolean(nullable: false));
            AddColumn("dbo.AnnouncementOffers", "IsFinished", c => c.Boolean(nullable: false));
            AddColumn("dbo.JobOffers", "IsAccepted", c => c.Boolean(nullable: false));
            AddColumn("dbo.JobOffers", "IsFinished", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobOffers", "IsFinished");
            DropColumn("dbo.JobOffers", "IsAccepted");
            DropColumn("dbo.AnnouncementOffers", "IsFinished");
            DropColumn("dbo.AnnouncementOffers", "IsAccepted");
        }
    }
}
