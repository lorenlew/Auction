namespace Auction.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
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
