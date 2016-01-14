using System;
using System.Collections.Generic;
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

namespace Projekt_BD.Views
{
    /// <summary>
    /// Interaction logic for PanelDodawaniaPacjenta.xaml
    /// </summary>
    public partial class PanelDodawaniaPacjenta : UserControl
    {
        private DbContext dbContext;
        public PanelDodawaniaPacjenta()
        {
            InitializeComponent();
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            using (dbContext = new DbContext())
            {
                dbContext.Pacjentci.Add(new Models.Pacjent { Pesel = TPesel.Text, Imie = tImie.Text, DataUrodzenie = tUrodzenie.SelectedDate.GetValueOrDefault(), Nazwisko = tNazwisko.Text, Mail = tMail.Text });
                dbContext.SaveChanges();
                MessageBox.Show("Pomyślnie dodano pacjenta!");
            }
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }


    }
}
