namespace GestionCyberCafe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsCoucheToSessionPoste : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SessionPostes", "IsCouche", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SessionPostes", "IsCouche");
        }
    }
}
