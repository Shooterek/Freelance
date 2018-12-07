namespace Freelance.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDecimalToInt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AnnouncementOffers", "ProposedRate", c => c.Int(nullable: false));
            AlterColumn("dbo.Announcements", "ExpectedHourlyWage", c => c.Int(nullable: false));
            AlterColumn("dbo.JobOffers", "ProposedRate", c => c.Int(nullable: false));
            AlterColumn("dbo.Jobs", "MinimumWage", c => c.Int(nullable: false));
            AlterColumn("dbo.Jobs", "MaximumWage", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Jobs", "MaximumWage", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Jobs", "MinimumWage", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.JobOffers", "ProposedRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Announcements", "ExpectedHourlyWage", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.AnnouncementOffers", "ProposedRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
