using Projekt_BD.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Linq.SqlClient;
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

                var pac = from pacjentcis in context.Pacjentci select new { pacjentcis.Pesel, pacjentcis.Imie, pacjentcis.Nazwisko, pacjentcis.DataUrodzenie.Year, Wiek = DateTime.Today.Year - pacjentcis.DataUrodzenie.Year, pacjentcis.MiejsceUrodzenia, pacjentcis.Mail };
                dataGrid_Pacienci.Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action(() => dataGrid_Pacienci.ItemsSource = pac.ToList()));

                var lek = from leki in context.SpisLekow select new { leki.NazwaLeku, leki.NazwaPolskaLeku };
                dataGrid_Leki.Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action(() => dataGrid_Leki.ItemsSource = lek.ToList()));

                var wiz = from wizyty in context.Wizyty
                          select new { wizyty.CzasWizyty, Pacjent = wizyty.Pacjent.Imie + " " + wizyty.Pacjent.Nazwisko,
                              wizyty.Data, Lekarz = wizyty.Lekarz.Imie + " " + wizyty.Lekarz.Nazwisko };
                dataGrid_Wizyty.Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action(() => dataGrid_Wizyty.ItemsSource = wiz.ToList()));

                var cho = from choroby in context.SpisChorob select new { choroby.NazwaChoroby, choroby.NazwaPolskaChoroby, choroby.Objawy, choroby.Opis, choroby.SposobyLeczenia };
                dataGrid_Choroby.Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action(() => dataGrid_Choroby.ItemsSource = cho.ToList()));
            }
        }
        private void PrzegladajBazeButton_Click(object sender, RoutedEventArgs e) {
            if (!przegladajBazeWorker.IsBusy)
                przegladajBazeWorker.RunWorkerAsync();
        }
        private void ObsluzWizyteButton_Click(object sender, RoutedEventArgs e) {
            MenuItemName.Content = "Obsługa Wizyty";
            CenterPanel1.Visibility = Visibility.Hidden;
        }
        private void ZarzadzajWizytamiButton_Click(object sender, RoutedEventArgs e) {
            MenuItemName.Content = "Zarządanie Wizytami";
            CenterPanel1.Visibility = Visibility.Hidden;
        }

        private void UstalGodzinyPrzyjecButton_Click(object sender, RoutedEventArgs e) {
            MenuItemName.Content = "Godziny Przyjęć";
            CenterPanel1.Visibility = Visibility.Hidden;
        }

        private void DodajPacjentaButton_Click(object sender, RoutedEventArgs e) {
            (new DodajPacjentaWindow()).ShowDialog();
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
            Data.Text = typ.GetProperty("Data").GetValue(objekt).ToString();
            CzasWizyty.Text = typ.GetProperty("CzasWizyty").GetValue(objekt).ToString();
            tLekarz.Text = typ.GetProperty("Lekarz").GetValue(objekt).ToString();
            tPacjenta.Text = typ.GetProperty("Pacjent").GetValue(objekt).ToString();
        }

        private void dataGrid_Pacienci_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            var objekt = dataGrid_Pacienci.SelectedItem;
            var typ = objekt.GetType();
            tImie.Text = typ.GetProperty("Imie").GetValue(objekt).ToString();
            tNazwisko.Text = typ.GetProperty("Nazwisko").GetValue(objekt).ToString();
            PESEL.Text = typ.GetProperty("Pesel").GetValue(objekt).ToString();
            DataUrodzenia.Text = typ.GetProperty("Year").GetValue(objekt).ToString();
            MiejsceUrodzenia.Text = typ.GetProperty("MiejsceUrodzenia").GetValue(objekt).ToString();
            
        }
    }
}
