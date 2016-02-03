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
using System.Windows.Threading;

namespace Projekt_BD.Views
{
    /// <summary>
    /// Interaction logic for GridPanel.xaml
    /// </summary>
    public partial class GridPanel : UserControl
    {
        public GridPanel()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {

            // Do not load your data at design time.
            // if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            // {
            // 	//Load your data here and assign the result to the CollectionViewSource.
            // 	System.Windows.Data.CollectionViewSource myCollectionViewSource = (System.Windows.Data.CollectionViewSource)this.Resources["Resource Key for CollectionViewSource"];
            // 	myCollectionViewSource.Source = your data
            // }
        }

        private void Grid_Loaded_1(object sender, RoutedEventArgs e)
        {
            CollectionViewSource wizytyViewSource = ((CollectionViewSource)(this.FindResource("wizytasViewSource")));
            CollectionViewSource pacjentViewSource = ((CollectionViewSource)(this.FindResource("pacjentsViewSource")));
            CollectionViewSource lekarzeViewSource = ((CollectionViewSource)(this.FindResource("lekarzsViewSource")));
            CollectionViewSource dniPrzyjecViewSource = ((CollectionViewSource)(this.FindResource("dniPrzyjecsViewSource")));


            using (DbContext context = new DbContext())
            {
                var wizyty = from wizyta in context.Wizyty select wizyta;
                wizytyViewSource.Source = wizyty.ToList();

                var pac = from pacjentcis in context.Pacjentci select pacjentcis;
                pacjentViewSource.Source = pac.ToList();

                var lekarze = from Lekarzs in context.Lekarze select Lekarzs;
                lekarzeViewSource.Source = lekarze.ToList();

                var przyjecia = from DniPrzyjecs in context.DniPrzyjec select DniPrzyjecs;
                dniPrzyjecViewSource.Source = przyjecia.ToList();

            } 

        }

        private void bEdytuj_Click(object sender, RoutedEventArgs e)
        {
            var objekt = pacjentsDataGrid.SelectedItem;
            if (objekt != null)
            {
                var pesel = objekt.GetType().GetProperty("Pesel").GetValue(objekt).ToString();
                (new EdytujPacjentaWindow(pesel)).ShowDialog();
            }
        }

        private void bDodaj_Click(object sender, RoutedEventArgs e)
        {
            (new DodajPacjentaWindow()).ShowDialog();
        }
    }
}
