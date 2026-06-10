namespace GestionCyberCafe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateModel : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.AbonnementInternets", "IdClient");
            AddForeignKey("dbo.AbonnementInternets", "IdClient", "dbo.Clients", "IdClient", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AbonnementInternets", "IdClient", "dbo.Clients");
            DropIndex("dbo.AbonnementInternets", new[] { "IdClient" });
        }
    }
}
