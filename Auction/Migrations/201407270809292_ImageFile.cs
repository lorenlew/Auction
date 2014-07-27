namespace Auction.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImageFile : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Lots", "ImagePath");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Lots", "ImagePath", c => c.String(nullable: false));
        }
    }
}
