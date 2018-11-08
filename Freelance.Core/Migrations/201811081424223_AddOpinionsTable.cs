namespace Freelance.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOpinionsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Opinions",
                c => new
                    {
                        OpinionId = c.Int(nullable: false, identity: true),
                        EvaluatedUserId = c.String(maxLength: 128),
                        Rating = c.Int(nullable: false),
                        Review = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.OpinionId)
                .ForeignKey("dbo.AspNetUsers", t => t.EvaluatedUserId)
                .Index(t => t.EvaluatedUserId);
            
            AddColumn("dbo.AspNetUsers", "Rating", c => c.Double(nullable: false));
            AddColumn("dbo.AspNetUsers", "AmountOfReviews", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Opinions", "EvaluatedUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Opinions", new[] { "EvaluatedUserId" });
            DropColumn("dbo.AspNetUsers", "AmountOfReviews");
            DropColumn("dbo.AspNetUsers", "Rating");
            DropTable("dbo.Opinions");
        }
    }
}
