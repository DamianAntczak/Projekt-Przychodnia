namespace Projekt_BD.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pacjents",
                c => new
                    {
                        IdPacjenta = c.Guid(nullable: false),
                        Imie = c.String(),
                        DataUrodzenie = c.DateTime(nullable: false),
                        MiejsceUrodzenia = c.String(),
                        Plec = c.String(),
                        NrTelefonu = c.String(),
                        Mail = c.String(),
                    })
                .PrimaryKey(t => t.IdPacjenta);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Pacjents");
        }
    }
}
