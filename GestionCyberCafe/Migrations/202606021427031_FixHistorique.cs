namespace GestionCyberCafe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixHistorique : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.SessionPostes", "IsCouche");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SessionPostes", "IsCouche", c => c.Boolean(nullable: false));
        }
    }
}
