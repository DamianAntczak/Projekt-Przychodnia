using Projekt_BD.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Projekt_BD.Views {
    /// <summary>
    /// Interaction logic for MainPanel.xaml
    /// </summary>
    public partial class MainPanel : Window {
        readonly BackgroundWorker przegladajBazeWorker = new BackgroundWorker();
        public MainPanel() {
            przegladajBazeWorker.DoWork += PrzegladajBazeWorker_DoWork;
            przegladajBazeWorker.RunWorkerCompleted += PrzegladajBazeWorker_RunWorkerCompleted;
            InitializeComponent();

        }

        private void PrzegladajBazeWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
        }

        private void PrzegladajBazeWorker_DoWork(object sender, DoWorkEventArgs e) {
            PrzegladajBazeMethod();
        }

        private void PrzegladajBazeMethod() {
            MenuItemName.Dispatcher.Invoke(DispatcherPriority.Normal,
                new Action(() => MenuItemName.Content = "Przeglądanie Bazy"));
            CenterPanel1.Dispatcher.Invoke(DispatcherPriority.Normal,
                new Action(() => CenterPanel1.Visibility = Visibility.Visible));

            using (var context = new DbContext()) {

                var pac = from pacjentcis in context.Pacjentci select new { pacjentcis.Pesel, pacjentcis.Imie, pacjentcis.Nazwisko, pacjentcis.DataUrodzenie.Year, Wiek = DateTime.Today.Year - pacjentcis.DataUrodzenie.Year, pacjentcis.MiejsceUrodzenia, pacjentcis.Mail, pacjentcis.NrTelefonu };
                dataGrid_Pacienci.Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action(() => dataGrid_Pacienci.ItemsSource = pac.ToList()));

                var lek = from leki in context.SpisLekow select new { leki.NazwaLeku, leki.NazwaPolskaLeku };
                dataGrid_Leki.Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action(() => dataGrid_Leki.ItemsSource = lek.ToList()));

                var wizyty = from wizyta in context.Wizyty select new { wizyta.IdWizyty, wizyta.Data, wizyta.CzasWizyty, wizyta.Recepty };
                dataGrid_Wizyty.Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action(() => dataGrid_Wizyty.ItemsSource = wizyty.ToList()));

                var cho = from choroby in context.SpisChorob select new { choroby.NazwaChoroby, choroby.NazwaPolskaChoroby, choroby.Objawy, choroby.Opis, choroby.SposobyLeczenia };
                dataGrid_Choroby.Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action(() => dataGrid_Choroby.ItemsSource = cho.ToList()));
            }
        }
        private void PrzegladajBazeButton_Click(object sender, RoutedEventArgs e) {
            Panele.Content = null;
            if (!przegladajBazeWorker.IsBusy)
                przegladajBazeWorker.RunWorkerAsync();

        }
        private void ObsluzWizyteButton_Click(object sender, RoutedEventArgs e) {
            MenuItemName.Content = "Obsługa Wizyty";
            CenterPanel1.Visibility = Visibility.Hidden;
            Panele.Content = null;
        }
        private void ZarzadzajWizytamiButton_Click(object sender, RoutedEventArgs e) {
            MenuItemName.Content = "Zarządanie Wizytami";
            
            PanelZarzadzaniaWizyta pzw = new PanelZarzadzaniaWizyta();
            CenterPanel1.Visibility = Visibility.Hidden;
            this.Panele.Content = pzw;
            pzw.VerticalAlignment = VerticalAlignment.Top;
            pzw.HorizontalAlignment = HorizontalAlignment.Left;
        }

        private void UstalGodzinyPrzyjecButton_Click(object sender, RoutedEventArgs e) {
            MenuItemName.Content = "Godziny Przyjęć";

            var pdp = new PanelDodawaniaPrzyjec();
            CenterPanel1.Visibility = Visibility.Hidden;
            this.Panele.Content = pdp;
            pdp.VerticalAlignment = VerticalAlignment.Top;
            pdp.HorizontalAlignment = HorizontalAlignment.Left;
     
        }

        private void DodajPacjentaButton_Click(object sender, RoutedEventArgs e) {
            (new DodajPacjentaWindow()).ShowDialog();
            if (!przegladajBazeWorker.IsBusy) {
                przegladajBazeWorker.RunWorkerAsync();
            }

        }
        #region TextChaned nieaktywne - nie można było używać selekcji w datagrid do wypisywania danych pacjenta
        //aby odkomentować zaznacz całość i użyj skrótów ctrl+k , ctrl+u
        //private void Imie_TextChanged(object sender, TextChangedEventArgs e) {
        //    using (var context = new DbContext()) {
        //        var pact = from pacjent in context.Pacjentci where pacjent.Imie.Contains(tImie.Text) select pacjent;

        //        dataGrid_Pacienci.ItemsSource = pact.ToList();
        //    }
        //}

        //private void Nazwisko_TextChanged(object sender, TextChangedEventArgs e) {
        //    using (var context = new DbContext()) {
        //        var pact = from pacjent in context.Pacjentci where pacjent.Nazwisko.Contains(tNazwisko.Text) select pacjent;

        //        dataGrid_Pacienci.ItemsSource = pact.ToList();
        //    }

        //}
        #endregion
        private void Window_KeyUp(object sender, KeyEventArgs e) {
            if (e.Key == Key.Escape)
                Close();
        }
        private DataGridRow gdr = new DataGridRow();

        private void dataGrid_Wizyty_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            var objekt = dataGrid_Wizyty.SelectedItem;
            //zabezpieczeniem przed wybraniem ostatniego rekordu (nie można rzutować na typ Wizyta)
            var typ = objekt.GetType();
            var idWizyty = typ.GetProperty("IdWizyty").GetValue(objekt);
            if (typ.GetProperty("Data").GetValue(objekt) != null)
                Data.Text = typ.GetProperty("Data").GetValue(objekt).ToString();
            if (typ.GetProperty("CzasWizyty").GetValue(objekt) != null)
                CzasWizyty.Text = typ.GetProperty("CzasWizyty").GetValue(objekt).ToString();
            //niektore wizyty maja nula w idlekarza- poprawic
            using (var context = new DbContext()) {
                var wizyta = from w in context.Wizyty
                             where w.IdWizyty == (Guid)idWizyty
                             select w;
                var pesel = wizyta.Select(w => w.HistoriaChoroby.Pesel).FirstOrDefault();
                if (pesel != null) {
                    var idLekarza = wizyta.Select(w => w.HistoriaChoroby.IdLekarza).FirstOrDefault();
                    tPacjenta.Text = pesel;
                    tLekarz.Text = idLekarza.ToString();
                }
            }
        }

        private void dataGrid_Pacienci_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            var objekt = dataGrid_Pacienci.SelectedItem;
            var typ = objekt.GetType();
            if (typ.GetProperty("Imie").GetValue(objekt) != null)
                tImie.Text = typ.GetProperty("Imie").GetValue(objekt).ToString();
            if (typ.GetProperty("Nazwisko").GetValue(objekt) != null)
                tNazwisko.Text = typ.GetProperty("Nazwisko").GetValue(objekt).ToString();
            if (typ.GetProperty("Pesel").GetValue(objekt) != null)
                PESEL.Text = typ.GetProperty("Pesel").GetValue(objekt).ToString();
            if (typ.GetProperty("Year").GetValue(objekt) != null)
                DataUrodzenia.Text = typ.GetProperty("Year").GetValue(objekt) != null ? typ.GetProperty("Year").GetValue(objekt).ToString() : " ";
            if (typ.GetProperty("MiejsceUrodzenia").GetValue(objekt) != null)
                MiejsceUrodzenia.Text = typ.GetProperty("MiejsceUrodzenia").GetValue(objekt).ToString();
            //if (typ.GetProperty("NrTelefonu").GetValue(objekt) != null)
            //    NrTelefonu.Text = typ.GetProperty("NrTelefonu").GetValue(objekt).ToString();
        }

        private void dataGrid_Choroby_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            var objekt = dataGrid_Choroby.SelectedItem;
            var typ = objekt.GetType();
            if (typ.GetProperty("NazwaChoroby").GetValue(objekt) != null)
                NazwaChoroby.Text = typ.GetProperty("NazwaChoroby").GetValue(objekt).ToString();
            if (typ.GetProperty("Opis").GetValue(objekt) != null)
                OpisChoroby.Text = typ.GetProperty("Opis").GetValue(objekt).ToString();
            if (typ.GetProperty("Objawy").GetValue(objekt) != null)
                Objawy.Text = typ.GetProperty("Objawy").GetValue(objekt).ToString();
        }

        private void dataGrid_Leki_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            var objekt = dataGrid_Leki.SelectedItem;
            var typ = objekt.GetType();
            if (typ.GetProperty("NazwaLeku").GetValue(objekt) != null)
                NazwaLeku.Text = typ.GetProperty("NazwaLeku").GetValue(objekt).ToString();
        }

        private void bEdytuj_Click(object sender, RoutedEventArgs e) {
            var objekt = dataGrid_Pacienci.SelectedItem;
            var pesel = objekt.GetType().GetProperty("Pesel").GetValue(objekt).ToString();
            (new EdytujPacjentaWindow(pesel)).ShowDialog();
        }

        private void ZarzadzajPacjentamiButton_Click(object sender, RoutedEventArgs e) {
            MenuItemName.Content = "Zarządanie Pacjentem";
            CenterPanel1.Visibility = Visibility.Hidden;
            PanelZarzadzaniaacjentem pzp = new PanelZarzadzaniaacjentem();
            Panele.Content = pzp;
            pzp.VerticalAlignment = VerticalAlignment.Top;
        }

        private void CenterPanel1_GotFocus(object sender, RoutedEventArgs e) {
        }
    }
}
