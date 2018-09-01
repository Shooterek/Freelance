namespace Freelance.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPublicationDateAndTitleProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Jobs", "Title", c => c.String());
            AddColumn("dbo.Jobs", "PublicationDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Jobs", "PublicationDate");
            DropColumn("dbo.Jobs", "Title");
        }
    }
}
