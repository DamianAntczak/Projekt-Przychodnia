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
        const string wczytajDane = "Wczytaj dane";
        const string wczytajLekarzy = "Wczytaj lekarzy";
        public PanelZarzadzaniaacjentem() {
            worker.DoWork += Worker_DoWork;
            InitializeComponent();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e) {
            if (e.Argument.ToString() == wczytajDane) {
                using (DbContext db = new DbContext()) {
                    var pac = from Pacjent in db.Pacjentci select Pacjent;
                    PacjentComboBox.Dispatcher.Invoke(DispatcherPriority.Normal,
                        new Action(() => PacjentComboBox.ItemsSource = pac.ToList()));
                }
            }
            else if (e.Argument.ToString() == wczytajLekarzy) {
                using (DbContext db = new DbContext()) {
                    Pacjent pacjent = null;
                    PacjentComboBox.Dispatcher.Invoke(DispatcherPriority.Normal,
                        new Action(() => pacjent = PacjentComboBox.SelectedItem as Pacjent));
                    if (pacjent != null) {
                        //wybrać tylko tych lekarzy którzy nie są już przypisani. 
                        var lekarz = from l in db.Lekarze
                                     where !( from l2 in db.Lekarze
                                              join hc in db.HistoriaChoroby
                                              on l2.IdLekarza equals hc.IdLekarza
                                              where hc.Pesel == pacjent.Pesel
                                              select l2.IdLekarza).Contains(l.IdLekarza)
                                     select l;

                        if (lekarz != null)
                            LekarzComboBox.Dispatcher.Invoke(DispatcherPriority.Normal,
                                new Action(() => LekarzComboBox.ItemsSource = lekarz.ToList()));
                    }
                }
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
                worker.RunWorkerAsync(wczytajDane);
            }
        }

        private void PacjentComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            LekarzComboBox.ItemsSource = null;
            if (PacjentComboBox.SelectedItem != null)
                while (true)
                    if (!worker.IsBusy) {
                        worker.RunWorkerAsync(wczytajLekarzy);
                        break;
                    }
        }
    }
}
