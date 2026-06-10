namespace GestionCyberCafe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_IsDeleted_And_TempsRestant_SessionWifi : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SessionWifis", "TempsRestantMinutes", c => c.Int());
            AddColumn("dbo.SessionWifis", "CreatedAt", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SessionWifis", "CreatedAt");
            DropColumn("dbo.SessionWifis", "TempsRestantMinutes");
        }
    }
}
