namespace Freelance.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPhotosToAnnouncementsAndJobs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Photos",
                c => new
                    {
                        PhotoId = c.Int(nullable: false, identity: true),
                        ContentType = c.String(maxLength: 31),
                        Content = c.Binary(),
                        AnnouncementId = c.Int(),
                        JobId = c.Int(),
                    })
                .PrimaryKey(t => t.PhotoId)
                .ForeignKey("dbo.Jobs", t => t.JobId)
                .ForeignKey("dbo.Announcements", t => t.AnnouncementId)
                .Index(t => t.AnnouncementId)
                .Index(t => t.JobId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Photos", "AnnouncementId", "dbo.Announcements");
            DropForeignKey("dbo.Photos", "JobId", "dbo.Jobs");
            DropIndex("dbo.Photos", new[] { "JobId" });
            DropIndex("dbo.Photos", new[] { "AnnouncementId" });
            DropTable("dbo.Photos");
        }
    }
}
