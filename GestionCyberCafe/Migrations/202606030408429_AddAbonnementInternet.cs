namespace GestionCyberCafe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAbonnementInternet : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AbonnementInternets",
                c => new
                    {
                        IdAbonnementInternet = c.Int(nullable: false, identity: true),
                        IdClient = c.Int(nullable: false),
                        TypeAcces = c.String(),
                        TypeForfait = c.String(),
                        DateDebut = c.DateTime(nullable: false),
                        DateExpiration = c.DateTime(nullable: false),
                        NombreAppareils = c.Int(nullable: false),
                        DureeMois = c.Int(nullable: false),
                        DureeAnnees = c.Int(nullable: false),
                        Montant = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Statut = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdAbonnementInternet);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AbonnementInternets");
        }
    }
}
