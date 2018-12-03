namespace Freelance.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLastActivationField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Announcements", "LastActivation", c => c.DateTime(nullable: false));
            AddColumn("dbo.Jobs", "LastActivation", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Jobs", "LastActivation");
            DropColumn("dbo.Announcements", "LastActivation");
        }
    }
}
