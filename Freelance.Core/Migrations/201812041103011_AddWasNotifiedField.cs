namespace Freelance.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWasNotifiedField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Announcements", "WasNotified", c => c.Boolean(nullable: false));
            AddColumn("dbo.Jobs", "WasNotified", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Jobs", "WasNotified");
            DropColumn("dbo.Announcements", "WasNotified");
        }
    }
}
