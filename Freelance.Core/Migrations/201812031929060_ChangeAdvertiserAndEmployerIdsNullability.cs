namespace Freelance.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeAdvertiserAndEmployerIdsNullability : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Announcements", "AdvertiserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Jobs", "EmployerId", "dbo.AspNetUsers");
            DropIndex("dbo.Announcements", new[] { "AdvertiserId" });
            DropIndex("dbo.Jobs", new[] { "EmployerId" });
            AlterColumn("dbo.Announcements", "AdvertiserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Jobs", "EmployerId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Announcements", "AdvertiserId");
            CreateIndex("dbo.Jobs", "EmployerId");
            AddForeignKey("dbo.Announcements", "AdvertiserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Jobs", "EmployerId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Jobs", "EmployerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Announcements", "AdvertiserId", "dbo.AspNetUsers");
            DropIndex("dbo.Jobs", new[] { "EmployerId" });
            DropIndex("dbo.Announcements", new[] { "AdvertiserId" });
            AlterColumn("dbo.Jobs", "EmployerId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Announcements", "AdvertiserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Jobs", "EmployerId");
            CreateIndex("dbo.Announcements", "AdvertiserId");
            AddForeignKey("dbo.Jobs", "EmployerId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Announcements", "AdvertiserId", "dbo.AspNetUsers", "Id");
        }
    }
}
