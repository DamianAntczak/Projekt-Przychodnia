using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Entity;
using Projekt_BD.Migrations;
using System.ComponentModel;
using System.Windows.Threading;
using Projekt_BD.Models;
using Projekt_BD.Views;

namespace Projekt_BD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DbContext dbContext;
        readonly BackgroundWorker worker = new BackgroundWorker();
        public MainWindow()
        {
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
           // (new Login()).ShowDialog(); //okno logowania
            InitializeComponent();
        }
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            StatusTworzeniaBazy.Dispatcher.Invoke(DispatcherPriority.Normal,
                new Action(() => StatusTworzeniaBazy.Content = "Czekaj..."));
            PierwszaFunkcja();
        }
        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            StatusTworzeniaBazy.Dispatcher.Invoke(DispatcherPriority.Normal,
                new Action(() => StatusTworzeniaBazy.Content = "Zakończono"));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            worker.RunWorkerAsync();

        }
        private void PierwszaFunkcja()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DbContext, Configuration>());

            using (var db = new DbContext())
            {
                //db.Database.Delete();
                Guid id = Guid.NewGuid();

                var jan = new Pacjent { IdPacjenta = id, Imie = "Jan", DataUrodzenie = new DateTime(1994, 1, 6), Mail = "rower@op.pl" };

                db.Pacjentci.Add(jan);

                var kardio = new Specjalizacja { IdSpecjalizacji = Guid.NewGuid(), Nazwa = "Kardiologia" };

                db.Specjalizacje.Add(kardio);

                var drMarcin = new Lekarz { IdLekarza = Guid.NewGuid(), Adres = "Kwiatowa 12", Imie = "Marcin", Nazwisko = "Nowak", Specjalizacja = kardio };

                db.Lekarze.Add(drMarcin);

                var uzytkownik = new Uzytkownik { UzytkownikId = Guid.NewGuid(), Haslo = "abc", Login = "damian" };

                db.Uzytkownicy.Add(uzytkownik);
                var lek1 = new Lek { IdLeku = Guid.NewGuid(), Nazwa = "Aspiryna", Producent = "x", StopienRefundacji = 0 };
                var lek2 = new Lek { IdLeku = Guid.NewGuid(), Nazwa = "Clotrimazor", Producent = "Farma", StopienRefundacji = 0 };
                db.Leki.Add(lek1);
                db.Leki.Add(lek2);
                var idPac = Guid.NewGuid();
                db.Pacjentci.Add(new Pacjent { IdPacjenta = idPac, Imie = "Radosław", Nazwisko = "Nowak", Plec = "M", DataUrodzenie = new DateTime(1985, 12, 11), MiejsceUrodzenia = "Poznań", Mail = "radoslaw.nowak@wp.pl" });
                db.Lekarze.Add(new Lekarz { IdLekarza = Guid.NewGuid(), Imie = "Arkadiusz", Nazwisko = "Kowalski", Adres = "Kwiatowa 33, Poznan" });
                //db.Choroby.Add(new Choroba { IdChoroby = Guid.NewGuid(), Nazwa = "Astma", Objawy = "Kaszel, zapowietrzenie", SposobyLeczenia = "Leki, świeże powietrze" });
               // db.Choroby.Add(new Choroba { IdChoroby = Guid.NewGuid(), Nazwa = "Zapalenie migdałków", Objawy = "Ból gardła, powiększone węzły chłonne", SposobyLeczenia = "Wycięcie migdałków" });
                //db.Specjalizacje.Add(new Specjalizacja { IdSpecjalizacji = Guid.NewGuid(), Nazwa = "Ginekolog" });
                //db.Specjalizacje.Add(new Specjalizacja { IdSpecjalizacji = Guid.NewGuid(), Nazwa = "Lekarz pierwszej pomocy" });
              // var pac = db.Pacjentci.Select(p => p.Imie == "Radosław").First();
                
              // db.Wizyty.Add(new Wizyta { PacjentId = pac., Data = new DateTime(2015, 09, 13), IdWizyty = Guid.NewGuid() });
             //  db.Wizyty.Add(new Wizyta { PacjentId = idPac, Data = new DateTime(2015, 11, 12), IdWizyty = Guid.NewGuid() });

                int i = db.SaveChanges();
                dbContext = db;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
