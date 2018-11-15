namespace Freelance.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMessageFieldToOffers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AnnouncementOffers", "Message", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.JobOffers", "Message", c => c.String(nullable: false, maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobOffers", "Message");
            DropColumn("dbo.AnnouncementOffers", "Message");
        }
    }
}
