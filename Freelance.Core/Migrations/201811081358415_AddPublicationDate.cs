namespace Freelance.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPublicationDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Announcements", "PublicationDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Announcements", "PublicationDate");
        }
    }
}
