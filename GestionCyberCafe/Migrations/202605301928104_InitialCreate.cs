namespace GestionCyberCafe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Abonnements",
                c => new
                    {
                        IdAbonnement = c.Int(nullable: false, identity: true),
                        Nom = c.String(),
                        Prix = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DureeMois = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdAbonnement);
            
            CreateTable(
                "dbo.Activites",
                c => new
                    {
                        IdActivite = c.Int(nullable: false, identity: true),
                        DateHeure = c.DateTime(nullable: false),
                        TypeActivite = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.IdActivite);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        IdClient = c.Int(nullable: false, identity: true),
                        Nom = c.String(),
                        Prenom = c.String(),
                        Telephone = c.String(),
                        Adresse = c.String(),
                        Statut = c.String(),
                    })
                .PrimaryKey(t => t.IdClient);
            
            CreateTable(
                "dbo.Paiements",
                c => new
                    {
                        IdPaiement = c.Int(nullable: false, identity: true),
                        IdSessionPoste = c.Int(nullable: false),
                        Montant = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ModePaiement = c.String(),
                        DatePaiement = c.DateTime(nullable: false),
                        Statut = c.String(),
                    })
                .PrimaryKey(t => t.IdPaiement);
            
            CreateTable(
                "dbo.Parametres",
                c => new
                    {
                        IdParametre = c.Int(nullable: false, identity: true),
                        NomCyber = c.String(),
                        Adresse = c.String(),
                        Telephone = c.String(),
                        PrixHeurePC = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PrixMinutePC = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PrixHeureWifi = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PrixMinuteWifi = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.IdParametre);
            
            CreateTable(
                "dbo.Postes",
                c => new
                    {
                        IdPoste = c.Int(nullable: false, identity: true),
                        NomPoste = c.String(),
                        Description = c.String(),
                        Statut = c.String(),
                    })
                .PrimaryKey(t => t.IdPoste);
            
            CreateTable(
                "dbo.SessionPostes",
                c => new
                    {
                        IdSessionPoste = c.Int(nullable: false, identity: true),
                        IdClient = c.Int(nullable: false),
                        IdPoste = c.Int(nullable: false),
                        HeureDebut = c.DateTime(nullable: false),
                        HeureFin = c.DateTime(),
                        DureeMinutes = c.Int(nullable: false),
                        ProlongationMinutes = c.Int(nullable: false),
                        MontantTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Statut = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdSessionPoste);
            
            CreateTable(
                "dbo.SessionWifis",
                c => new
                    {
                        IdSessionWifi = c.Int(nullable: false, identity: true),
                        IdClient = c.Int(nullable: false),
                        MarqueAppareil = c.String(),
                        TypeReseau = c.String(),
                        HeureDebut = c.DateTime(nullable: false),
                        HeureFin = c.DateTime(),
                        DureeMinutes = c.Int(nullable: false),
                        MontantTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Statut = c.String(),
                    })
                .PrimaryKey(t => t.IdSessionWifi);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SessionWifis");
            DropTable("dbo.SessionPostes");
            DropTable("dbo.Postes");
            DropTable("dbo.Parametres");
            DropTable("dbo.Paiements");
            DropTable("dbo.Clients");
            DropTable("dbo.Activites");
            DropTable("dbo.Abonnements");
        }
    }
}
