using System.Data.Entity.Migrations;

namespace Auction.DAL.Migrations
{
    public partial class fixStakeTimeout : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stakes", "StakeTimeout", c => c.DateTime(nullable: false));
            DropColumn("dbo.Stakes", "HoursForAuctionEnd");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Stakes", "HoursForAuctionEnd", c => c.Int(nullable: false));
            DropColumn("dbo.Stakes", "StakeTimeout");
        }
    }
}
