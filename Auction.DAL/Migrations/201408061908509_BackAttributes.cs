namespace Auction.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BackAttributes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Lots", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Lots", "ImagePath", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Lots", "ImagePath", c => c.String());
            AlterColumn("dbo.Lots", "Name", c => c.String(maxLength: 100));
        }
    }
}
