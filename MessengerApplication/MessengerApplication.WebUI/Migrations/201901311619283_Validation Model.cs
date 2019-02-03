namespace MessengerApplication.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ValidationModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Messages", "MessageData", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Messages", "MessageData", c => c.String());
        }
    }
}
