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
    /// Interaction logic for PanelDodawaniaPrzyjec.xaml
    /// </summary>
    public partial class PanelDodawaniaPrzyjec : UserControl
    {
        private List<String> godziny;

        public PanelDodawaniaPrzyjec()
        {
            godziny = new List<string>();

            for (int i = 8; i <= 20; i++)
            {
                godziny.Add(i+":00");
                godziny.Add(i + ":15");
                godziny.Add(i + ":30");
                godziny.Add(i + ":45");
            }

            InitializeComponent();

            rozpoczecieBox.ItemsSource = godziny;
            zakoczenieBox.ItemsSource = godziny;

            using (DbContext db = new DbContext())
            {
                var lekarz = db.Lekarze.First();

                DniPrzyjec dniPrzyjec = new DniPrzyjec();
                dniPrzyjec.Lekarz = lekarz;
                //dniPrzyjec.CzasDatarRozpoczecia = Kalendarz.SelectedDate.Value;
                //dniPrzyjec.CzasPrzyjec = TimeSpan.Parse("8h");

            }
        }
    }
}
