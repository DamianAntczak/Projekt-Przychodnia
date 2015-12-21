namespace Projekt_BD.Migrations {
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Projekt_BD.DbContext> {
        public Configuration() {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "Projekt_BD.DbContext";
        }

        protected override void Seed(Projekt_BD.DbContext context) {
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
            var pacjent2 = new Pacjent { IdPacjenta = Guid.NewGuid(), Imie = "Janusz", Nazwisko = "Polewski", DataUrodzenie = new DateTime(1992, 12, 5), MiejsceUrodzenia = "Katowice", Plec = "M�czyzna", NrTelefonu = "987654321", Mail = "janusz@bd.pl" };
            context.Set<Pacjent>().AddOrUpdate(pacjent);
            context.Set<Pacjent>().AddOrUpdate(pacjent2);

            var specjalizacja = new Specjalizacja { IdSpecjalizacji = Guid.NewGuid(), Nazwa = "Kardiologia" };
            //nazwa specjalizacji lekarza rodzinnego (patrz. wikipedia)
            var specjalizacja2 = new Specjalizacja { IdSpecjalizacji = Guid.NewGuid(), Nazwa = "Medycyna Og�lna" };
            context.Set<Specjalizacja>().AddOrUpdate(specjalizacja);
            context.Set<Specjalizacja>().AddOrUpdate(specjalizacja2);

            var lekarz = new Lekarz { IdLekarza = Guid.NewGuid(), Imie = "Konrad", Nazwisko = "Jab�o�ski", Specjalizacja = specjalizacja, Adres = "ul. Warszawska 1, 62-220 Pozna�" };
            var lekarz2 = new Lekarz { IdLekarza = Guid.NewGuid(), Imie = "Ma�gorzata", Nazwisko = "G�rniak", Specjalizacja = specjalizacja2, Adres = "ul. Bohater�w II Wojny �wiatowej 5, 61-330 Pozna�" };
            context.Set<Lekarz>().AddOrUpdate(lekarz);
            context.Set<Lekarz>().AddOrUpdate(lekarz2);


            var choroba = new SpisChorb { NazwaChoroby = "Angina", Opis = "https://pl.wikipedia.org/wiki/Angina", NazwaPolskaChoroby = "Angina", Objawy = "Gor�czka, b�l gard�a, os�abienie, brak apetytu", SposobyLeczenia = "Kuracja antybiotykowa, przebywanie w ciep�ym miejscu" };
            var choroba2 = new SpisChorb { NazwaChoroby = "Hypertonia Arterialis", Opis = "https://pl.wikipedia.org/wiki/Nadci%C5%9Bnienie_t%C4%99tnicze", NazwaPolskaChoroby = "Nadci�nienie t�tnicze", Objawy = "B�l g�owy, zawroty g�owy, bezsenno��", SposobyLeczenia = "Przyjmowanie lekarstw na nadci�nienie, spacer na �wie�ym powietrzu" };
            context.Set<SpisChorb>().AddOrUpdate(choroba);
            context.Set<SpisChorb>().AddOrUpdate(choroba2);

            context.Set<Choroba>().AddOrUpdate(new Choroba { IdChoroby = Guid.NewGuid(), ChorobaZeSpisu = choroba, Pacjent = pacjent, IdPacjenta = pacjent.IdPacjenta });
            context.Set<Choroba>().AddOrUpdate(new Choroba { IdChoroby = Guid.NewGuid(), ChorobaZeSpisu = choroba2, Pacjent = pacjent2, IdPacjenta = pacjent2.IdPacjenta });

            var lek = new SpisLekow { NazwaLeku = "Amoxicillinum", NazwaPolskaLeku = "amoksycylina" };
            var lek2 = new SpisLekow { NazwaLeku = "Simvastatinum ", NazwaPolskaLeku = "symwastatyna" };
            context.Set<SpisLekow>().AddOrUpdate(lek, lek2);

            var wizyta = new Wizyta { IdWizyty = Guid.NewGuid(), CzasWizyty = new TimeSpan(0, 20, 0), Data = new DateTime(2015, 09, 18), Lekarz = lekarz, Pacjent = pacjent };
            var wizyta2 = new Wizyta { IdWizyty = Guid.NewGuid(), CzasWizyty = new TimeSpan(0, 34, 0), Data = new DateTime(2015, 09, 18), Lekarz = lekarz2, Pacjent = pacjent2 };
            context.Set<Wizyta>().AddOrUpdate(wizyta, wizyta2);

            var recepta = new Recepta { IdRecepty = Guid.NewGuid(), Wizyta = wizyta, CzasWystawienia = new DateTime(2015, 09, 18, 12, 12, 12) };
            var recepta2 = new Recepta { IdRecepty = Guid.NewGuid(), Wizyta = wizyta2, CzasWystawienia = new DateTime(2015, 09, 18, 12, 12, 12) };
            context.Set<Recepta>().AddOrUpdate(recepta, recepta2);

            context.Set<Lek>().AddOrUpdate(new Lek { IdLeku = Guid.NewGuid(), Dawka = "5mg", Przyjmowanie = "2 razy dziennie", StopienRefundacji = 0, SpisLekow = lek2, Recepta = recepta },
                new Lek { IdLeku = Guid.NewGuid(), Dawka = "7mg", Przyjmowanie = "3 razy dziennie", StopienRefundacji = 0, SpisLekow = lek2, Recepta = recepta2 });
        }
    }
}