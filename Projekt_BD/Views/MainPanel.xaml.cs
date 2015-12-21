﻿using System;
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

namespace Projekt_BD.Views {
    /// <summary>
    /// Interaction logic for MainPanel.xaml
    /// </summary>
    public partial class MainPanel : Window {
        public MainPanel() {
            InitializeComponent();

        }

        private void PrzegladajBazeButton_Click(object sender, RoutedEventArgs e) {
            MenuItemName.Content = "Przeglądanie Bazy";
            CenterPanel1.Visibility = Visibility.Visible;

            using (var context = new DbContext()) {

                var pac = from pacjentcis in context.Pacjentci select pacjentcis;

                dataGrid_Pacienci.ItemsSource = pac.ToList();

                var lek = from leki in context.Leki select leki;

                dataGrid_Leki.ItemsSource = lek.ToList();

                var wiz = from wizyty in context.Wizyty select wizyty;
                dataGrid_Wizyty.ItemsSource = wiz.ToList();

                var cho = from choroby in context.Choroby select choroby;
                dataGrid_Choroby.ItemsSource = cho.ToList();
            }


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

        private void Imie_TextChanged(object sender, TextChangedEventArgs e) {
            using (var context = new DbContext()) {
                var pact = from pacjent in context.Pacjentci where pacjent.Imie.Contains(tImie.Text) select pacjent;

                dataGrid_Pacienci.ItemsSource = pact.ToList();
            }
        }

        private void Nazwisko_TextChanged(object sender, TextChangedEventArgs e) {
            using (var context = new DbContext()) {
                var pact = from pacjent in context.Pacjentci where pacjent.Nazwisko.Contains(tNazwisko.Text) select pacjent;

                dataGrid_Pacienci.ItemsSource = pact.ToList();
            }

        }

        private void Window_KeyUp(object sender, KeyEventArgs e) {
            Close();
        }
    }
}
