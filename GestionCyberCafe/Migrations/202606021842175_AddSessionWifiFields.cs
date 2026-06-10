namespace GestionCyberCafe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSessionWifiFields : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.SessionWifis", "IdClient");
            AddForeignKey("dbo.SessionWifis", "IdClient", "dbo.Clients", "IdClient", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SessionWifis", "IdClient", "dbo.Clients");
            DropIndex("dbo.SessionWifis", new[] { "IdClient" });
        }
    }
}
