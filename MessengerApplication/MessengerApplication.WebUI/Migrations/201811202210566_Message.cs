namespace MessengerApplication.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Message : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageId = c.Int(nullable: false, identity: true),
                        SendTime = c.DateTime(nullable: false),
                        ReceiveTime = c.DateTime(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                        SenderName = c.String(),
                        ReceiverName = c.String(),
                        MessageData = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Messages", new[] { "ApplicationUser_Id" });
            DropTable("dbo.Messages");
        }
    }
}
