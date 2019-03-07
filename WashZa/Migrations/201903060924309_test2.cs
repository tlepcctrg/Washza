namespace WashZa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.WashLaundries", "flag_payment", c => c.Boolean(nullable: false));
            AlterColumn("dbo.WashLaundries", "flag_send", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WashLaundries", "flag_send", c => c.String());
            AlterColumn("dbo.WashLaundries", "flag_payment", c => c.String());
        }
    }
}
