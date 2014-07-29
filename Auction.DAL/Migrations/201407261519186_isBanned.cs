using System.Data.Entity.Migrations;

namespace Auction.DAL.Migrations
{
    public partial class isBanned : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "IsBanned", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "IsBanned");
        }
    }
}
