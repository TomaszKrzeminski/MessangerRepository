namespace MessengerApplication.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "ApplicationUserId", c => c.Int(nullable: false));
            AlterColumn("dbo.Messages", "SendTime", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Messages", "ReceiveTime", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Messages", "ReceiveTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Messages", "SendTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Messages", "ApplicationUserId");
        }
    }
}
