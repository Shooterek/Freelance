namespace Freelance.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveUnusedProperties : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "Rating");
            DropColumn("dbo.AspNetUsers", "AmountOfReviews");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "AmountOfReviews", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "Rating", c => c.Double(nullable: false));
        }
    }
}
