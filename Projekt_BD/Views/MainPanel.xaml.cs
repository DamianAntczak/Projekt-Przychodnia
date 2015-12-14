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
