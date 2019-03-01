namespace WashZa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newfields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "First_Name", c => c.String());
            AddColumn("dbo.AspNetUsers", "Last_Name", c => c.String());
            AddColumn("dbo.AspNetUsers", "Address", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Address");
            DropColumn("dbo.AspNetUsers", "Last_Name");
            DropColumn("dbo.AspNetUsers", "First_Name");
        }
    }
}
