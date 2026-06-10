namespace GestionCyberCafe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTarifsAbonnements : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ParametreConfigs", "PrixSemaineWifi", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ParametreConfigs", "PrixMoisWifi", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ParametreConfigs", "PrixAnneeWifi", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ParametreConfigs", "PrixAnneeWifi");
            DropColumn("dbo.ParametreConfigs", "PrixMoisWifi");
            DropColumn("dbo.ParametreConfigs", "PrixSemaineWifi");
        }
    }
}
