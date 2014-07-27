namespace Auction.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImagePathBack : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Lots", "ImagePath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Lots", "ImagePath");
        }
    }
}
