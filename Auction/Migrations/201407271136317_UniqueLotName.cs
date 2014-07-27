namespace Auction.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UniqueLotName : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Lots", "Name", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Lots", new[] { "Name" });
        }
    }
}
