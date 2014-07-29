namespace Auction.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdChange : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Stakes", "LotId", "dbo.Lots");
            DropIndex("dbo.Lots", new[] { "Name" });
            DropPrimaryKey("dbo.Lots");
            DropPrimaryKey("dbo.Stakes");
            DropColumn("dbo.Lots", "LotId");
            DropColumn("dbo.Stakes", "StakeId");
            AddColumn("dbo.Lots", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Stakes", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Lots", "Id");
            AddPrimaryKey("dbo.Stakes", "Id");
            AddForeignKey("dbo.Stakes", "LotId", "dbo.Lots", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            AddColumn("dbo.Stakes", "StakeId", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Lots", "LotId", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.Stakes", "LotId", "dbo.Lots");
            DropPrimaryKey("dbo.Stakes");
            DropPrimaryKey("dbo.Lots");
            DropColumn("dbo.Stakes", "Id");
            DropColumn("dbo.Lots", "Id");
            AddPrimaryKey("dbo.Stakes", "StakeId");
            AddPrimaryKey("dbo.Lots", "LotId");
            CreateIndex("dbo.Lots", "Name", unique: true);
            AddForeignKey("dbo.Stakes", "LotId", "dbo.Lots", "Id", cascadeDelete: true);
        }
    }
}
