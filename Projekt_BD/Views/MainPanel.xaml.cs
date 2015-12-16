﻿using System;
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
using System.Windows.Shapes;

namespace Projekt_BD.Views
{
    /// <summary>
    /// Interaction logic for MainPanel.xaml
    /// </summary>
    public partial class MainPanel : Window
    {
        public MainPanel()
        {
            InitializeComponent();

        }

        private void PrzegladajBazeButton_Click(object sender, RoutedEventArgs e)
        {
            MenuItemName.Content = "Przeglądanie Bazy";
            CenterPanel1.Visibility = Visibility.Visible;

            using (var context = new DbContext())
            {

                var pac = from pacjentcis in context.Pacjentci select pacjentcis;

                dataGrid_Pacienci.ItemsSource = pac.ToList();

                var lek = from leki in context.Leki select leki;

                dataGrid_Leki.ItemsSource = lek.ToList();

                var wiz = from wizyty in context.Wizyty select wizyty;
                dataGrid_Leki.ItemsSource = wiz.ToList();

                var cho = from choroby in context.Choroby select choroby;
                dataGrid_Leki.ItemsSource = cho.ToList();
            }
        

        }
        private void ObsluzWizyteButton_Click(object sender, RoutedEventArgs e)
        {
            MenuItemName.Content = "Obsługa Wizyty";
            CenterPanel1.Visibility = Visibility.Hidden;
        }
        private void ZarzadzajWizytamiButton_Click(object sender, RoutedEventArgs e)
        {
            MenuItemName.Content = "Zarządanie Wizytami";
            CenterPanel1.Visibility = Visibility.Hidden;
        }

        private void UstalGodzinyPrzyjecButton_Click(object sender, RoutedEventArgs e)
        {
            MenuItemName.Content = "Godziny Przyjęć";
            CenterPanel1.Visibility = Visibility.Hidden;
        }

        private void DodajPacjentaButton_Click(object sender, RoutedEventArgs e)
        {
            (new DodajPacjentaWindow()).ShowDialog();
        }
    }
}
