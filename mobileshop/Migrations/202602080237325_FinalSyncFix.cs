namespace mobileshop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FinalSyncFix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "ProductName", c => c.String(nullable: false));
            AddColumn("dbo.Products", "Image", c => c.String());
            DropColumn("dbo.Products", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Name", c => c.String(nullable: false));
            DropColumn("dbo.Products", "Image");
            DropColumn("dbo.Products", "ProductName");
        }
    }
}
