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
using System.Data.Entity;
using Projekt_BD.Migrations;

namespace Projekt_BD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DbContext, Configuration>());

            using (var db = new DbContext())
            {
                Guid id = Guid.NewGuid();

                var jan = new Pacjent{ IdPacjenta = id, Imie= "Jan", DataUrodzenie = new DateTime(1994,1,6),Mail = "rower@op.pl"};

                db.Pacjentci.Add(jan);

                var drMarcin = new Lekarz{IdLekarza = Guid.NewGuid(),Adres = "Kwiatowa 12",Imie = "Marcin", Nazwisko = "Nowak"};

                db.Lekarze.Add(drMarcin);

                int i = db.SaveChanges();
            }

        }
    }
}
