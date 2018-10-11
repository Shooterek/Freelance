namespace Freelance.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAvailabilityColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Announcements", "Availability", c => c.Int(nullable: false));
            AddColumn("dbo.Jobs", "Availability", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Jobs", "Availability");
            DropColumn("dbo.Announcements", "Availability");
        }
    }
}
