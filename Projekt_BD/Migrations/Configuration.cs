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

            var pacjent = new Pacjent { Pesel = "11686911222", Imie = "Katarzyna", Nazwisko = "Janicka", DataUrodzenie = new DateTime(1984, 1, 4), MiejsceUrodzenia = "Opole", Plec = Plec.Mezczyzna, NrTelefonu = "123456789", Mail = "katarzyna@bd.pl" };
            var pacjent2 = new Pacjent { Pesel = "24686111254", Imie = "Janusz", Nazwisko = "Polewski", DataUrodzenie = new DateTime(1992, 12, 5), MiejsceUrodzenia = "Katowice", Plec = Plec.Kobieta, NrTelefonu = "987654321", Mail = "janusz@bd.pl" };
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


            var choroba = new SpisChorb { NazwaChoroby = "Angina", Opis = "https://pl.wikipedia.org/wiki/Angina", NazwaPolskaChoroby = "Angina", Objawy = "Gor¹czka, ból gard³a, os³abienie, brak apetytu", SposobyLeczenia = "Kuracja antybiotykowa, przebywanie w ciep³ym miejscu" };
            var choroba2 = new SpisChorb { NazwaChoroby = "Hypertonia Arterialis", Opis = "https://pl.wikipedia.org/wiki/Nadci%C5%9Bnienie_t%C4%99tnicze", NazwaPolskaChoroby = "Nadciœnienie têtnicze", Objawy = "Ból g³owy, zawroty g³owy, bezsennoœæ", SposobyLeczenia = "Przyjmowanie lekarstw na nadciœnienie, spacer na œwie¿ym powietrzu" };
            context.Set<SpisChorb>().AddOrUpdate(choroba);
            context.Set<SpisChorb>().AddOrUpdate(choroba2);

            var histChoroby1 = new HistoriaChoroby {
                ChorobaZeSpisu = choroba,
                IdLekarza = lekarz.IdLekarza,
                Pesel = pacjent.Pesel,
                OpisChoroby = "Angina u pacjenta ma typowe objawy i przebiega w normalny sposób",
                OstatniaModyfikacjaOpisuChoroby = DateTime.Today
            };
            var histChoroby2 = new HistoriaChoroby {
                ChorobaZeSpisu = choroba2,
                IdLekarza = lekarz.IdLekarza,
                Pesel = pacjent2.Pesel,
                OpisChoroby = "Wysokie nadciœnienie u pacjenta.",
                OstatniaModyfikacjaOpisuChoroby = DateTime.Today
            };
            context.Set<HistoriaChoroby>().AddOrUpdate(histChoroby1);
            context.Set<HistoriaChoroby>().AddOrUpdate(histChoroby2);

            var lek = new SpisLekow { NazwaLeku = "Amoxicillinum", NazwaPolskaLeku = "amoksycylina" };
            var lek2 = new SpisLekow { NazwaLeku = "Simvastatinum ", NazwaPolskaLeku = "symwastatyna" };
            context.Set<SpisLekow>().AddOrUpdate(lek, lek2);

            var wizyta = new Wizyta { IdWizyty = Guid.NewGuid(), Data = new DateTime(2015, 09, 18), HistoriaChoroby = histChoroby1, CzasWizyty = CzasWizyty.getTimeSpan(Czas.min15) };
            var wizyta2 = new Wizyta { IdWizyty = Guid.NewGuid(), Data = new DateTime(2015, 09, 18), HistoriaChoroby = histChoroby2, CzasWizyty = CzasWizyty.getTimeSpan(Czas.min30)};
            context.Set<Wizyta>().AddOrUpdate(wizyta, wizyta2);

            var recepta = new Recepta { IdRecepty = Guid.NewGuid(), Wizyta = wizyta, CzasWystawienia = new DateTime(2015, 09, 18, 12, 12, 12) };
            var recepta2 = new Recepta { IdRecepty = Guid.NewGuid(), Wizyta = wizyta2, CzasWystawienia = new DateTime(2015, 09, 18, 12, 12, 12) };
            context.Set<Recepta>().AddOrUpdate(recepta, recepta2);

            context.Set<Lek>().AddOrUpdate(new Lek { IdLeku = Guid.NewGuid(), Dawka = "5mg", Przyjmowanie = "2 razy dziennie", StopienRefundacji = 0, SpisLekow = lek2, Recepta = recepta },
                new Lek { IdLeku = Guid.NewGuid(), Dawka = "7mg", Przyjmowanie = "3 razy dziennie", StopienRefundacji = 0, SpisLekow = lek2, Recepta = recepta2 });
        }
    }
}