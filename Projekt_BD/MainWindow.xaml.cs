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

namespace Projekt_BD {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private DbContext dbContext;
        readonly BackgroundWorker worker = new BackgroundWorker();
        public MainWindow() {
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            // (new Login()).ShowDialog(); //okno logowania
            (new MainPanel()).ShowDialog();
            InitializeComponent();
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

        private void Button_Click_1(object sender, RoutedEventArgs e) {
            if (!worker.IsBusy)
                worker.RunWorkerAsync();

        }
        private void PierwszaFunkcja() {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DbContext, Configuration>());

            using (var db = new DbContext()) {
                //db.Database.Delete();
                Guid id = Guid.NewGuid();

                //var uzytkownik = new Uzytkownik { UzytkownikId = Guid.NewGuid(), Haslo = "abc", Login = "damian" };

                //db.Uzytkownicy.Add(uzytkownik);
                //db.Uzytkownicy.Add(new Uzytkownik { Login = "admin", Haslo = "admin", UzytkownikId = Guid.NewGuid() });
                //db.Pacjentci.Add(new Pacjent { IdPacjenta = Guid.NewGuid(), Imie = "Katarzyna", Nazwisko = "Jan", DataUrodzenie = new DateTime(1984, 1, 4), MiejsceUrodzenia = "Opole", Plec = "Kobieta", NrTelefonu = "123456789", Mail = "katarzyna@bd.pl" });
                //db.Lekarze.Add(new Lekarz());

                var i = db.SaveChanges();
                dbContext = db;
            }
        }

        private void Window_Closed(object sender, EventArgs e) {
            Application.Current.Shutdown();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e) {
            if (e.Key == Key.Escape)
                Application.Current.Shutdown();
        }
    }
}
