namespace GestionCyberCafe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTarifsAbonnement : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Parametres", newName: "ParametreConfigs");
            AddColumn("dbo.ParametreConfigs", "PrixSemainePoste", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ParametreConfigs", "PrixMoisPoste", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ParametreConfigs", "PrixAnneePoste", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ParametreConfigs", "PrixAnneePoste");
            DropColumn("dbo.ParametreConfigs", "PrixMoisPoste");
            DropColumn("dbo.ParametreConfigs", "PrixSemainePoste");
            RenameTable(name: "dbo.ParametreConfigs", newName: "Parametres");
        }
    }
}
