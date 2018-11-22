namespace Freelance.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLocalizationToJob : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Jobs", "Localization", c => c.String(maxLength: 31));
            AlterColumn("dbo.Announcements", "Localization", c => c.String(maxLength: 31));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Announcements", "Localization", c => c.String());
            DropColumn("dbo.Jobs", "Localization");
        }
    }
}
