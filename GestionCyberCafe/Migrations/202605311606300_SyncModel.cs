namespace GestionCyberCafe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SyncModel : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.SessionPostes", "IdClient");
            CreateIndex("dbo.SessionPostes", "IdPoste");
            AddForeignKey("dbo.SessionPostes", "IdClient", "dbo.Clients", "IdClient", cascadeDelete: true);
            AddForeignKey("dbo.SessionPostes", "IdPoste", "dbo.Postes", "IdPoste", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SessionPostes", "IdPoste", "dbo.Postes");
            DropForeignKey("dbo.SessionPostes", "IdClient", "dbo.Clients");
            DropIndex("dbo.SessionPostes", new[] { "IdPoste" });
            DropIndex("dbo.SessionPostes", new[] { "IdClient" });
        }
    }
}
