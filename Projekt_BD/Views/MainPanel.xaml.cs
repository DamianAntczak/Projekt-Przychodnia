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
        private readonly DbContext _context = new DbContext();

        private IEnumerable<Wizyta> _wizyty;

        public IEnumerable<Wizyta> Wizyty {
            get {
                return new ObservableCollection<Wizyta>(_context.Wizyty);
            }
            set {
                _wizyty = value;
            }
        }

        public MainPanel() {
            przegladajBazeWorker.DoWork += PrzegladajBazeWorker_DoWork;
            DataContext = this;
            InitializeComponent();
            MenuItemName.Content = "Witamy w systemie";

        }
        private void PrzegladajBazeWorker_DoWork(object sender, DoWorkEventArgs e) {
            PrzegladajBazeMethod();
        }

        private void PrzegladajBazeMethod() {
            MenuItemName.Dispatcher.Invoke(DispatcherPriority.Normal,
                new Action(() => MenuItemName.Content = "Przeglądanie Bazy"));
        }
        private void PrzegladajBazeButton_Click(object sender, RoutedEventArgs e) {
            Panele.Content = null;
            var pok = new GridPanel();
            MenuItemName.Content = "Przeglądanie bazy";
            this.Panele.Content = pok;
            pok.VerticalAlignment = VerticalAlignment.Top;
            pok.HorizontalAlignment = HorizontalAlignment.Left;

        }
        private void ObsluzWizyteButton_Click(object sender, RoutedEventArgs e) {
            var pok = new PanelObslugiWizyty();
            MenuItemName.Content = "Obsługa Wizyty";
            this.Panele.Content = pok;
            pok.VerticalAlignment = VerticalAlignment.Top;
            pok.HorizontalAlignment = HorizontalAlignment.Left;
        }
        private void ZarzadzajWizytamiButton_Click(object sender, RoutedEventArgs e) {
            MenuItemName.Content = "Zarządanie Wizytami";
            PanelZarzadzaniaWizyta pzw = new PanelZarzadzaniaWizyta();
            this.Panele.Content = pzw;
            pzw.VerticalAlignment = VerticalAlignment.Top;
            pzw.HorizontalAlignment = HorizontalAlignment.Left;
        }

        private void UstalGodzinyPrzyjecButton_Click(object sender, RoutedEventArgs e) {
            MenuItemName.Content = "Godziny Przyjęć";
            var pdp = new PanelDodawaniaPrzyjec();
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
        private void Window_KeyUp(object sender, KeyEventArgs e) {
            if (e.Key == Key.Escape)
                Close();
        }
        private DataGridRow gdr = new DataGridRow();
        private void Window_Initialized(object sender, EventArgs e) {
            if (!przegladajBazeWorker.IsBusy)
                przegladajBazeWorker.RunWorkerAsync();
        }
    }
}
