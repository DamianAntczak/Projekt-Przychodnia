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
        readonly BackgroundWorker worker = new BackgroundWorker();
        public GridPanel()
        {
            worker.DoWork += Worker_DoWork;
            InitializeComponent();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e) {
            CollectionViewSource wizytyViewSource = ((CollectionViewSource)(this.FindResource("wizytasViewSource")));
            CollectionViewSource pacjentViewSource = ((CollectionViewSource)(this.FindResource("pacjentsViewSource")));
            CollectionViewSource lekarzeViewSource = ((CollectionViewSource)(this.FindResource("lekarzsViewSource")));
            CollectionViewSource dniPrzyjecViewSource = ((CollectionViewSource)(this.FindResource("dniPrzyjecsViewSource")));


            using (DbContext context = new DbContext()) {
                var wizyty = from wizyta in context.Wizyty select wizyta;
                wizytyViewSource.Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action(() => wizytyViewSource.Source = wizyty.ToList()));

                var pac = from pacjentcis in context.Pacjentci select pacjentcis;
                pacjentViewSource.Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action(() => pacjentViewSource.Source = pac.ToList()));

                var lekarze = from Lekarzs in context.Lekarze select Lekarzs;
                lekarzeViewSource.Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action(() => lekarzeViewSource.Source = lekarze.ToList()));

                var przyjecia = from DniPrzyjecs in context.DniPrzyjec select DniPrzyjecs;
                dniPrzyjecViewSource.Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action(() => dniPrzyjecViewSource.Source = przyjecia.ToList()));

            }
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
            if(!worker.IsBusy)
                worker.RunWorkerAsync();    

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
