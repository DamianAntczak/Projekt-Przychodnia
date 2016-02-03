using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Projekt_BD.Models;
using System.Data.Entity;

namespace Projekt_BD.Views {
    /// <summary>
    /// Interaction logic for MainPanel.xaml
    /// </summary>
    public partial class MainPanel : Window {
        readonly BackgroundWorker przegladajBazeWorker = new BackgroundWorker();
        private  readonly DbContext  _context = new DbContext();

        private IEnumerable<Wizyta> _wizyty;

        public IEnumerable<Wizyta> Wizyty
        {
            get
            {
                return new ObservableCollection<Wizyta>(_context.Wizyty);
            }
            set
            {
                _wizyty = value;
                //OnPropertyChanged("Wizyty");
            }
        }

        public MainPanel() {
            przegladajBazeWorker.DoWork += PrzegladajBazeWorker_DoWork;
            przegladajBazeWorker.RunWorkerCompleted += PrzegladajBazeWorker_RunWorkerCompleted;
            InitializeComponent();
            DataContext = this;
            CenterPanel1.Visibility = Visibility.Hidden;
            MenuItemName.Content = "Witamy w systemie";

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
                new Action(() => CenterPanel1.Visibility = Visibility.Hidden));

            
        }
        private void PrzegladajBazeButton_Click(object sender, RoutedEventArgs e) {
            Panele.Content = null;
            if (!przegladajBazeWorker.IsBusy)
                przegladajBazeWorker.RunWorkerAsync();

            var pok = new GridPanel();
            MenuItemName.Content = "Przeglądanie bazy";
            CenterPanel1.Visibility = Visibility.Hidden;
            this.Panele.Content = pok;
            pok.VerticalAlignment = VerticalAlignment.Top;
            pok.HorizontalAlignment = HorizontalAlignment.Left;

        }
        private void ObsluzWizyteButton_Click(object sender, RoutedEventArgs e) {
            var pok = new PanelObslugiWizyty();
            MenuItemName.Content = "Obsługa Wizyty";
            CenterPanel1.Visibility = Visibility.Hidden;
            this.Panele.Content = pok;
            pok.VerticalAlignment = VerticalAlignment.Top;
            pok.HorizontalAlignment = HorizontalAlignment.Left;
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
            if (objekt != null) {
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
        }

        private void dataGrid_Pacienci_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            var objekt = dataGrid_Pacienci.SelectedItem;
            if (objekt != null) {
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
            if (objekt != null) {
                var pesel = objekt.GetType().GetProperty("Pesel").GetValue(objekt).ToString();
                (new EdytujPacjentaWindow(pesel)).ShowDialog();
            }
            if (!przegladajBazeWorker.IsBusy)
                przegladajBazeWorker.RunWorkerAsync();
        }


        private void CenterPanel1_GotFocus(object sender, RoutedEventArgs e) {
        }

        private void Window_Initialized(object sender, EventArgs e) {
            if (!przegladajBazeWorker.IsBusy)
                przegladajBazeWorker.RunWorkerAsync();
        }
    }
}
