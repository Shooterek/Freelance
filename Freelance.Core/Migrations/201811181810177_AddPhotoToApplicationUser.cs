namespace Freelance.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPhotoToApplicationUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "PhotoId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "PhotoId");
            AddForeignKey("dbo.AspNetUsers", "PhotoId", "dbo.Photos", "PhotoId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "PhotoId", "dbo.Photos");
            DropIndex("dbo.AspNetUsers", new[] { "PhotoId" });
            DropColumn("dbo.AspNetUsers", "PhotoId");
        }
    }
}
