using System.Data.Entity.Migrations;

namespace Auction.DAL.Migrations
{
    public partial class isSold : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Lots", "IsSold", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Lots", "IsSold");
        }
    }
}
