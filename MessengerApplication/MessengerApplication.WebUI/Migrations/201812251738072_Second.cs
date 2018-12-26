namespace MessengerApplication.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Second : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Messages", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Messages", new[] { "ApplicationUser_Id" });
            CreateTable(
                "dbo.ApplicationUserMessages",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Message_MessageId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Message_MessageId })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Messages", t => t.Message_MessageId, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Message_MessageId);
            
            DropColumn("dbo.Messages", "ApplicationUserId");
            DropColumn("dbo.Messages", "ApplicationUser_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Messages", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Messages", "ApplicationUserId", c => c.Int(nullable: false));
            DropForeignKey("dbo.ApplicationUserMessages", "Message_MessageId", "dbo.Messages");
            DropForeignKey("dbo.ApplicationUserMessages", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ApplicationUserMessages", new[] { "Message_MessageId" });
            DropIndex("dbo.ApplicationUserMessages", new[] { "ApplicationUser_Id" });
            DropTable("dbo.ApplicationUserMessages");
            CreateIndex("dbo.Messages", "ApplicationUser_Id");
            AddForeignKey("dbo.Messages", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
