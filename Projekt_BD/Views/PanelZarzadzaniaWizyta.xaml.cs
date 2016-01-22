using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Threading;

namespace Projekt_BD.Views {
    /// <summary>
    /// Interaction logic for PanelZarzadzaniaWizyta.xaml
    /// </summary>
    public partial class PanelZarzadzaniaWizyta : UserControl {
        readonly BackgroundWorker worker = new BackgroundWorker();
        const int maksymalnaDataDoTylu = -14;
        public PanelZarzadzaniaWizyta() {
            worker.DoWork += Worker_DoWork;
            InitializeComponent();
            WybierzDate.DisplayDateStart = DateTime.Today.AddDays(maksymalnaDataDoTylu);
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e) {
            using (var context = new DbContext()) {
                var pac = from pacjentcis in context.Pacjentci select new { pacjentcis.Pesel, pacjentcis.Imie, pacjentcis.Nazwisko, pacjentcis.DataUrodzenie.Year, Wiek = DateTime.Today.Year - pacjentcis.DataUrodzenie.Year, pacjentcis.MiejsceUrodzenia, pacjentcis.Mail };
                dataGrid_Pacjenci.Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action(() => dataGrid_Pacjenci.ItemsSource = pac.ToList()));
            }
        }

        private void PrzypiszPacjentaDoLekarza_Click(object sender, RoutedEventArgs e) {
            PanelZarzadzaniaacjentem pzp = new PanelZarzadzaniaacjentem();
            this.Content = pzp;
            pzp.VerticalAlignment = VerticalAlignment.Top;
        }

        private void PanelZarzadzaniaWizyta1_Initialized(object sender, EventArgs e) {
            if (!worker.IsBusy)
                worker.RunWorkerAsync();
        }

        private void dataGrid_Pacjenci_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            LekarzComboBox.SelectedItem = null;
            using (var context = new DbContext()) {
                var pacjent = dataGrid_Pacjenci.SelectedItem;
                var pesel = pacjent.GetType().GetProperty("Pesel").GetValue(pacjent).ToString();
                var lekarz = from l in context.Lekarze
                          join hc in context.HistoriaChoroby
                          on l.IdLekarza equals hc.IdLekarza
                          where hc.Pesel == pesel
                          select l;
                LekarzComboBox.ItemsSource = lekarz.ToList();   
            }
        }

        private void DodajNowaWizyte_Click(object sender, RoutedEventArgs e) {
            if (WybierzDate.SelectedDate.HasValue && LekarzComboBox.SelectedItem != null) {

                using (var context = new DbContext()) {
                    var lekarz = LekarzComboBox.SelectedItem as Lekarz;
                    var pacjent = dataGrid_Pacjenci.SelectedItem;
                    var pesel = pacjent.GetType().GetProperty("Pesel").GetValue(pacjent).ToString();
                    var historiaChoroby = from hc in context.HistoriaChoroby
                                          where hc.IdLekarza == lekarz.IdLekarza &&
                                          hc.Pesel == pesel
                                          select hc;
                    context.Wizyty.Add(new Models.Wizyta { IdWizyty = Guid.NewGuid(), Data = WybierzDate.SelectedDate.Value, HistoriaChoroby = historiaChoroby.FirstOrDefault() });
                    context.SaveChanges();
                }
                var button = sender as Button;
                button.Foreground = Brushes.Green;
                button.Content = "Pomyślnie zapisano.";
            }
            else
                MessageBox.Show("Proszę wybrać pacjenta, lekarza i datę wizyty");
        }

        private void LekarzComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            WizytaPacjenta.Content = "";
            var lekarz = LekarzComboBox.SelectedItem as Lekarz;
            if (lekarz != null) {
                using (var context = new DbContext()) {
                    var pacjent = dataGrid_Pacjenci.SelectedItem;
                    var pesel = pacjent.GetType().GetProperty("Pesel").GetValue(pacjent).ToString();
                    var wizyta = from w in context.Wizyty
                                 where w.HistoriaChoroby.IdLekarza == lekarz.IdLekarza &&
                                 w.HistoriaChoroby.Pesel == pesel
                                 select w;
                    foreach (var i in wizyta) {
                        WizytaPacjenta.Content += i.Data.ToString() + "\n";
                    }
                }
            }
        }

        private void DodajNowaWizyte_LostFocus(object sender, RoutedEventArgs e) {
            var button = sender as Button;
            button.Foreground = Brushes.Black;
            button.Content = "Dodaj nową wizytę";
        }
    }
}
