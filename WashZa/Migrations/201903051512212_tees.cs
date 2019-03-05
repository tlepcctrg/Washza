namespace WashZa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tees : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WashLaundries",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Active = c.Int(),
                        userid = c.String(),
                        payid = c.String(),
                        amount = c.Decimal(precision: 18, scale: 2),
                        flag_payment = c.String(),
                        flag_send = c.String(),
                        First_Name = c.String(),
                        Last_Name = c.String(),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WashLaundries");
        }
    }
}
