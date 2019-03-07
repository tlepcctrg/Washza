namespace WashZa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Tel", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Tel");
        }
    }
}
