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
using Projekt_BD.Models;
using System.ComponentModel;
using System.Windows.Threading;

namespace Projekt_BD.Views {
    /// <summary>
    /// Interaction logic for PanelZarzadzaniaacjentem.xaml
    /// </summary>
    public partial class PanelZarzadzaniaacjentem : UserControl {
        readonly BackgroundWorker worker = new BackgroundWorker();
        public PanelZarzadzaniaacjentem() {
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            InitializeComponent();
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e) {
            using (DbContext db = new DbContext()) {
                var pac = from Pacjent in db.Pacjentci select Pacjent;
                var lekarz = from Lekarz in db.Lekarze select Lekarz;
                PacjentComboBox.Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action(() => PacjentComboBox.ItemsSource = pac.ToList()));
                LekarzComboBox.Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action(() => LekarzComboBox.ItemsSource = lekarz.ToList()));
            }
        }

        private void Polacz_Click(object sender, RoutedEventArgs e) {
            var lekarz = (Lekarz)LekarzComboBox.SelectedItem;

            var pacjent = (Pacjent)PacjentComboBox.SelectedItem;


            using (DbContext db = new DbContext()) {
                db.HistoriaChoroby.Add(new Models.HistoriaChoroby { IdLekarza = lekarz.IdLekarza, Pesel = pacjent.Pesel, OstatniaModyfikacjaOpisuChoroby = DateTime.Now });
                db.SaveChanges();

                MessageBox.Show("Dodano " + pacjent.Imie + "" + pacjent.Nazwisko + " do pacjentów " + lekarz.Imie + " " + lekarz.Nazwisko);
            }
        }

        private void UserControl_Initialized(object sender, EventArgs e) {
            if (!worker.IsBusy) {
                worker.RunWorkerAsync();
            }
        }
    }
}
