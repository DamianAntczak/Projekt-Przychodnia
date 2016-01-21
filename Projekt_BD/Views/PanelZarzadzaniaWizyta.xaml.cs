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
        public PanelZarzadzaniaWizyta() {
            worker.DoWork += Worker_DoWork;
            InitializeComponent();
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
            using (var db = new DbContext()) {
                //var lekarz = from Lekarz in db.Lekarze select Lekarz ;
                var pacjent = dataGrid_Pacjenci.SelectedItem;
                var pesel = pacjent.GetType().GetProperty("Pesel").GetValue(pacjent).ToString();
                var lekarz = from l in db.Lekarze
                          join hc in db.HistoriaChoroby
                          on l.IdLekarza equals hc.IdLekarza
                          where hc.Pesel == pesel
                          select l;
                LekarzComboBox.ItemsSource = lekarz.ToList();
            }
        }
    }
}
