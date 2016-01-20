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
using Projekt_BD.Models;

namespace Projekt_BD.Views
{
    /// <summary>
    /// Interaction logic for PanelZarzadzaniaacjentem.xaml
    /// </summary>
    public partial class PanelZarzadzaniaacjentem : UserControl
    {
        public PanelZarzadzaniaacjentem()
        {
            InitializeComponent();

            using (DbContext db = new DbContext())
            {

                var pac = from Pacjent in db.Pacjentci select Pacjent;
                PacjentComboBox.ItemsSource = pac.ToList();

                var lekarz = from Lekarz in db.Lekarze select Lekarz;
                LekarzComboBox.ItemsSource = lekarz.ToList();
            }


        }

        private void Polacz_Click(object sender, RoutedEventArgs e)
        {
            var lekarz = (Lekarz)LekarzComboBox.SelectedItem;

            var pacjent = (Pacjent)PacjentComboBox.SelectedItem;


            using (DbContext db = new DbContext())
            {
                db.HistoriaChoroby.Add(new Models.HistoriaChoroby { IdLekarza = lekarz.IdLekarza, Pesel = pacjent.Pesel, OstatniaModyfikacjaOpisuChoroby = DateTime.Now});
                db.SaveChanges();

                MessageBox.Show("Dodano " + pacjent.Imie + "" + pacjent.Nazwisko + " do pacjentów "+lekarz.Imie+" "+lekarz.Nazwisko);
            }

        }
    }
}
