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
using System.Windows.Shapes;
using Projekt_BD.Models;

namespace Projekt_BD.Views
{
    /// <summary>
    /// Interaction logic for EdytujPacjentaWindow.xaml
    /// </summary>
    public partial class EdytujPacjentaWindow : Window
    {
        private  string pesel;
        private DbContext dbContext { get; set; }

        public EdytujPacjentaWindow(string pesel)
        {
            this.pesel = pesel;
            InitializeComponent();

            using (dbContext = new DbContext())
            {
                var x = (from Pacjent in dbContext.Pacjentci where Pacjent.Pesel == pesel select Pacjent).First();
                tImie.Text = x.Imie;

            }
        }

        private void bZapisz_Click(object sender, RoutedEventArgs e)
        {
            
        }

      
    }
}
