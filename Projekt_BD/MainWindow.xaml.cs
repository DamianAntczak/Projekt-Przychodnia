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
            (new Login()).ShowDialog(); //okno logowania
            InitializeComponent();
        }
        public MainWindow(bool close):this()
        {
            if (!close)
            {
                worker.DoWork += worker_DoWork;
                worker.RunWorkerCompleted += worker_RunWorkerCompleted;
                (new Login()).ShowDialog(); //okno logowania
                InitializeComponent();
            }
            else
            {
                this.Close();
            }
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e) {
            StatusTworzeniaBazy.Dispatcher.Invoke(DispatcherPriority.Normal,
                new Action(() => StatusTworzeniaBazy.Content = "Czekaj..."));
            PierwszaFunkcja();
        }
        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            StatusTworzeniaBazy.Dispatcher.Invoke(DispatcherPriority.Normal,
                new Action(() => StatusTworzeniaBazy.Content = "Zakończono"));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            worker.RunWorkerAsync();
        }
        private void PierwszaFunkcja() {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DbContext, Configuration>());

            using(var db = new DbContext()) {
                //db.Database.Delete();
                Guid id = Guid.NewGuid();

                var jan = new Pacjent { IdPacjenta = id, Imie = "Jan", DataUrodzenie = new DateTime(1994, 1, 6), Mail = "rower@op.pl" };

                db.Pacjentci.Add(jan);

                var kardio = new Specjalizacja { IdSpecjalizacji = Guid.NewGuid(), Nazwa = "Kardiologia" };

                db.Specjalizacje.Add(kardio);

                var drMarcin = new Lekarz { IdLekarza = Guid.NewGuid(), Adres = "Kwiatowa 12", Imie = "Marcin", Nazwisko = "Nowak", Specjalizacja = kardio };

                db.Lekarze.Add(drMarcin);

                var uzytkownik = new Uzytkownik {UzytkownikId = Guid.NewGuid(), Haslo = "abc", Login = "damian"};

                db.Uzytkownicy.Add(uzytkownik);



                int i = db.SaveChanges();
                dbContext = db;
            }
        }


    }
}
