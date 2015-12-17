namespace Projekt_BD.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Projekt_BD.DbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "Projekt_BD.DbContext";
        }

        protected override void Seed(Projekt_BD.DbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            var pacjent = new Pacjent { IdPacjenta = Guid.NewGuid(), Imie = "Katarzyna", Nazwisko = "Janicka", DataUrodzenie = new DateTime(1984, 1, 4), MiejsceUrodzenia = "Opole", Plec = "Kobieta", NrTelefonu = "123456789", Mail = "katarzyna@bd.pl" };
            var pacjent2 = new Pacjent { IdPacjenta = Guid.NewGuid(), Imie = "Janusz", Nazwisko = "Polewski", DataUrodzenie = new DateTime(1992, 12, 5), MiejsceUrodzenia = "Katowice", Plec = "Mê¿czyzna", NrTelefonu = "987654321", Mail = "janusz@bd.pl" };
            context.Set<Pacjent>().AddOrUpdate(pacjent);
            context.Set<Pacjent>().AddOrUpdate(pacjent2);

            var specjalizacja = new Specjalizacja { IdSpecjalizacji = Guid.NewGuid(), Nazwa = "Kardiologia" };
            //nazwa specjalizacji lekarza rodzinnego (patrz. wikipedia)
            var specjalizacja2 = new Specjalizacja { IdSpecjalizacji = Guid.NewGuid(), Nazwa = "Medycyna Ogólna" };
            context.Set<Specjalizacja>().AddOrUpdate(specjalizacja);
            context.Set<Specjalizacja>().AddOrUpdate(specjalizacja2);
            
            var lekarz = new Lekarz { IdLekarza = Guid.NewGuid(), Imie = "Konrad", Nazwisko = "Jab³oñski", Specjalizacja = specjalizacja, Adres = "ul. Warszawska 1, 62-220 Poznañ" };
            var lekarz2 = new Lekarz { IdLekarza = Guid.NewGuid(), Imie = "Ma³gorzata", Nazwisko = "Górniak", Specjalizacja = specjalizacja2, Adres = "ul. Bohaterów II Wojny Œwiatowej 5, 61-330 Poznañ" };
            context.Set<Lekarz>().AddOrUpdate(lekarz);
            context.Set<Lekarz>().AddOrUpdate(lekarz2);

            
            var choroba = new Choroba { IdChoroby = Guid.NewGuid(), IdPacjenta = pacjent.IdPacjenta, Pacjent = pacjent, Opis= "https://pl.wikipedia.org/wiki/Angina", Nazwa = "Angina", Objawy = "Gor¹czka, ból gard³a, os³abienie, brak apetytu", SposobyLeczenia = "Kuracja antybiotykowa, przebywanie w ciep³ym miejscu" };
            var choroba2 = new Choroba { IdChoroby = Guid.NewGuid(), IdPacjenta = pacjent2.IdPacjenta, Pacjent = pacjent2, Opis = "https://pl.wikipedia.org/wiki/Nadci%C5%9Bnienie_t%C4%99tnicze", Nazwa = "Nadciœnienie têtnicze", Objawy = "Ból g³owy, zawroty g³owy, bezsennoœæ", SposobyLeczenia = "Przyjmowanie lekarstw na nadciœnienie, spacer na œwie¿ym powietrzu" };
            context.Set<Choroba>().AddOrUpdate(choroba);
            context.Set<Choroba>().AddOrUpdate(choroba2);
        }
    }
}
