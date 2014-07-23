namespace Auction.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Corrected_UserToStakeConnection : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Lots", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Lots", new[] { "ApplicationUser_Id" });
            AlterColumn("dbo.Stakes", "ApplicationUserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Stakes", "ApplicationUserId");
            AddForeignKey("dbo.Stakes", "ApplicationUserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            DropColumn("dbo.Lots", "ApplicationUser_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Lots", "ApplicationUser_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.Stakes", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Stakes", new[] { "ApplicationUserId" });
            AlterColumn("dbo.Stakes", "ApplicationUserId", c => c.String(nullable: false));
            CreateIndex("dbo.Lots", "ApplicationUser_Id");
            AddForeignKey("dbo.Lots", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
