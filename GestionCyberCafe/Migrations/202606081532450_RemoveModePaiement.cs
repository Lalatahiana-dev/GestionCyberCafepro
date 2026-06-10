namespace GestionCyberCafe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveModePaiement : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Paiements", "ModePaiement");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Paiements", "ModePaiement", c => c.String());
        }
    }
}
