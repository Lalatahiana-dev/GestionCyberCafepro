namespace GestionCyberCafe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_IsDeleted_SessionWifi : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SessionWifis", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SessionWifis", "IsDeleted");
        }
    }
}
