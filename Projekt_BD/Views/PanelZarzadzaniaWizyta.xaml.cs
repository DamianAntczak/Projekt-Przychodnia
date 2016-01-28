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
        readonly BackgroundWorker worker_Pacjent = new BackgroundWorker();
        readonly BackgroundWorker worker_Lekarz = new BackgroundWorker();
        const string godz = "GG";
        const string min = "MM";
        const string uruchomLekarza = "lekarz";
        const string uruchomPacjenta = "pacjent";
        const int maksymalnaDataDoTylu = -14;
        public PanelZarzadzaniaWizyta() {
            worker_Pacjent.DoWork += Worker_Pacjent_DoWork;
            worker_Lekarz.DoWork += Worker_Lekarz_DoWork;
            InitializeComponent();
            WybierzDate.DisplayDateStart = DateTime.Today.AddDays(maksymalnaDataDoTylu);
        }

        private void Worker_Lekarz_DoWork(object sender, DoWorkEventArgs e) {
            if(e.Argument.ToString() == uruchomLekarza)
                WczytajLekarzy();
            else if(e.Argument.ToString() == uruchomPacjenta) {
                WczytajPacjentowLekarza();
            }
        }

        private void Worker_Pacjent_DoWork(object sender, DoWorkEventArgs e) {
            using (var context = new DbContext()) {
                var pac = from pacjentcis in context.Pacjentci select new { pacjentcis.Pesel, pacjentcis.Imie, pacjentcis.Nazwisko, pacjentcis.DataUrodzenie.Year, Wiek = DateTime.Today.Year - pacjentcis.DataUrodzenie.Year, pacjentcis.MiejsceUrodzenia, pacjentcis.Mail };
                dataGrid_Pacjenci.Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action(() => dataGrid_Pacjenci.ItemsSource = pac.ToList()));
            }
            List<Projekt_BD.Models.Czas> czas = new List<Models.Czas>() { Models.Czas.min15, Models.Czas.min30, Models.Czas.min45, Models.Czas.h1, Models.Czas.h2 };
            CzasComboBox.Dispatcher.Invoke(DispatcherPriority.Normal,
                new Action(() => CzasComboBox.ItemsSource = czas));
        }
        private void WczytajLekarzy() {
            using (var context = new DbContext()) {
                var lekarz = context.Lekarze.ToList();
                WybierzLekarza.Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action(() => WybierzLekarza.ItemsSource = lekarz));
            }
        }
        private void WczytajPacjentowLekarza() {
            using (var context = new DbContext()) {
                Lekarz lekarz = null;
                WybierzLekarza.Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action(() => lekarz = WybierzLekarza.SelectedItem as Lekarz));
                if (lekarz != null) {
                    var pacjent = from p in context.Pacjentci
                                  join hc in context.HistoriaChoroby
                                  on p.Pesel equals hc.Pesel
                                  where hc.IdLekarza == lekarz.IdLekarza
                                  select new { p.Pesel, p.Imie, p.Nazwisko };
                    PacjenciLekarza.Dispatcher.Invoke(DispatcherPriority.Normal,
                        new Action(() => PacjenciLekarza.ItemsSource = pacjent.ToList()));
                }
            }
        }
        private void PrzypiszPacjentaDoLekarza_Click(object sender, RoutedEventArgs e) {
            PanelZarzadzaniaacjentem pzp = new PanelZarzadzaniaacjentem();
        }

        private void PanelZarzadzaniaWizyta1_Initialized(object sender, EventArgs e) {
            if (!worker_Pacjent.IsBusy)
                worker_Pacjent.RunWorkerAsync();
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
            if (WybierzDate.SelectedDate.HasValue && LekarzComboBox.SelectedItem != null && CzasComboBox != null) {
                var czas = (Models.Czas)CzasComboBox.SelectedItem;
                // d = Enum.GetValues(Models.Czas);
                using (var context = new DbContext()) {
                    var lekarz = LekarzComboBox.SelectedItem as Lekarz;
                    var pacjent = dataGrid_Pacjenci.SelectedItem;
                    var pesel = pacjent.GetType().GetProperty("Pesel").GetValue(pacjent).ToString();
                    var historiaChoroby = from hc in context.HistoriaChoroby
                                          where hc.IdLekarza == lekarz.IdLekarza &&
                                          hc.Pesel == pesel
                                          select hc;
                    var data = new DateTime();
                    data = WybierzDate.SelectedDate.Value;
                    data = data.AddHours(double.Parse(GodzinaWizyty.Text));
                    data = data.AddMinutes(double.Parse(MinutaWizyty.Text));
                    context.Wizyty.Add(new Models.Wizyta { IdWizyty = Guid.NewGuid(), Data = data, HistoriaChoroby = historiaChoroby.FirstOrDefault(), CzasWizyty = Models.CzasWizyty.getTimeSpan(czas) });
                    context.SaveChanges();
                }
                PrzesunCzas();
                var button = sender as Button;
                button.Foreground = Brushes.Green;
                button.Content = "Pomyślnie zapisano.";
                WyswietlListeWizyt();
            }
            else
                MessageBox.Show("Proszę wybrać pacjenta, lekarza i datę wizyty");
        }
        private void PrzesunCzas() {
            var dlugoscWizyty = (int)CzasComboBox.SelectedValue;
            var godzina = new TimeSpan(int.Parse(GodzinaWizyty.Text), int.Parse(MinutaWizyty.Text), 0);
            godzina = godzina.Add(new TimeSpan(0, dlugoscWizyty, 0));
            GodzinaWizyty.Text = godzina.Hours.ToString();
            MinutaWizyty.Text = godzina.Minutes.ToString();
        }
        private void LekarzComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            WyswietlListeWizyt();
        }
        private void WyswietlListeWizyt() {
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

        private void GodzinaWizyty_GotFocus(object sender, RoutedEventArgs e) {
            var textbox = (TextBox)sender;
            if (textbox.Text == godz) {
                textbox.Foreground = Brushes.Black;
                textbox.Text = "";
            }
        }

        private void GodzinaWizyty_LostFocus(object sender, RoutedEventArgs e) {
            var textbox = (TextBox)sender;
            if (textbox.Text == "") {
                textbox.Foreground = Brushes.Gray;
                textbox.Text = godz;
            }
        }

        private void MinutaWizyty_LostFocus(object sender, RoutedEventArgs e) {
            var textbox = (TextBox)sender;
            if (textbox.Text == "") {
                textbox.Foreground = Brushes.Gray;
                textbox.Text = min;
            }
        }

        private void MinutaWizyty_GotFocus(object sender, RoutedEventArgs e) {
            var textbox = (TextBox)sender;
            if (textbox.Text == min) {
                textbox.Foreground = Brushes.Black;
                textbox.Text = "";
            }
        }

        private void UstalWizytyLekarzowiTab_Initialized(object sender, EventArgs e) {
            worker_Lekarz.RunWorkerAsync(uruchomLekarza);
        }

        private void WybierzLekarza_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (!worker_Lekarz.IsBusy)
                worker_Lekarz.RunWorkerAsync(uruchomPacjenta);
        }
    }
}
