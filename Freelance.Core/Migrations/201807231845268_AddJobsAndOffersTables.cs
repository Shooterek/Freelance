namespace Freelance.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddJobsAndOffersTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Offers",
                c => new
                    {
                        OfferId = c.Int(nullable: false, identity: true),
                        JobId = c.Int(nullable: false),
                        OffererId = c.String(maxLength: 128),
                        ProposedRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.OfferId)
                .ForeignKey("dbo.Jobs", t => t.JobId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.OffererId)
                .Index(t => t.JobId)
                .Index(t => t.OffererId);
            
            CreateTable(
                "dbo.Jobs",
                c => new
                    {
                        JobId = c.Int(nullable: false, identity: true),
                        EmployerId = c.String(maxLength: 128),
                        ServiceTypeId = c.Int(nullable: false),
                        Description = c.String(),
                        MinimumWage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MaximumWage = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.JobId)
                .ForeignKey("dbo.AspNetUsers", t => t.EmployerId)
                .ForeignKey("dbo.ServiceTypes", t => t.ServiceTypeId, cascadeDelete: true)
                .Index(t => t.EmployerId)
                .Index(t => t.ServiceTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Offers", "OffererId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Jobs", "ServiceTypeId", "dbo.ServiceTypes");
            DropForeignKey("dbo.Offers", "JobId", "dbo.Jobs");
            DropForeignKey("dbo.Jobs", "EmployerId", "dbo.AspNetUsers");
            DropIndex("dbo.Jobs", new[] { "ServiceTypeId" });
            DropIndex("dbo.Jobs", new[] { "EmployerId" });
            DropIndex("dbo.Offers", new[] { "OffererId" });
            DropIndex("dbo.Offers", new[] { "JobId" });
            DropTable("dbo.Jobs");
            DropTable("dbo.Offers");
        }
    }
}
